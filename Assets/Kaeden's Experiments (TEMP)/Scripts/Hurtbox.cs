using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Hurtbox : MonoBehaviour, IDamagable
{
    //reference variables
    public IDamagable[] damagableScripts;
    public IScriptable[] scriptableScripts;
    public IUnique uniqueScript;

    //hitstun variables
    private float hitstunTimer;
    [HideInInspector]
    public bool inHitStun;
    public bool NoFlash;

    //misc variables
    private Animator animator;
    private AudioSource audiosrc;
    private SpriteRenderer[] sr;
    private Shader[] OriginalShader;
    public bool NoHitParticles;
    private IEnumerator running;
    private Color[] OriginalColors;
    public bool invin;

    //hitstop variables
    private GameObject HitEffect;
    private EffectsManager FXM; //used for hitstop
    private Shader HitShader;
    public bool FSMHitstun;
    public string default_ani;

    public void damage(AttackData ad)
    {
        //play hit audio if it exists
        //if (audiosrc != null && ad.audio != null) audiosrc.PlayOneShot(ad.audio, 0.5f);
        //Hit particle effect
        if (HitEffect != null && !NoHitParticles)
        {
            GameObject instance = Instantiate(HitEffect, transform.position, Quaternion.identity);
            instance.transform.SetParent(gameObject.transform);
            //set correct scale
            float XScale = instance.transform.localScale.x * gameObject.transform.localScale.x + (0.05f*ad.hitstop);
            float YScale = instance.transform.localScale.y * gameObject.transform.localScale.y + (0.05f*ad.hitstop);
            instance.transform.localScale = new Vector3(XScale, YScale, instance.transform.localScale.z);

            //play slower if hitstop is greater
            ParticleSystem ps = instance.GetComponent<ParticleSystem>();
            var main = ps.main;
            float Speed = main.simulationSpeed - ad.hitstop * 0.02f > 0 ? main.simulationSpeed - ad.hitstop * 0.02f: 0.001f;
            main.simulationSpeed = Speed;
        }

        //if not dead play hitstun animation
        HealthTracker ht = gameObject?.GetComponent<HealthTracker>();
        if (ht != null && ht.healthSystem.getHealth() > 0)
        {
            hitstunTimer = ad.hitstun;
            inHitStun = true;
            if (!FSMHitstun)
            {
                //if (agent != null) agent.enabled = false;
                //CHANGE LOGIC HERE TO ROUTE TO HITSTUN ANIMATION
                if (uniqueScript == null)
                {
                    if (animator != null) animator.Play(default_ani);
                }
                else
                {
                    uniqueScript.HitStunAni();
                }
            }
        }
        //get rid of hitboxes if you've just been hit (circumvent bug)
        AttackManager am = this?.GetComponent<AttackManager>();
        if (am != null)
        {
            am.DestroyPlay();
        }

        //deactivate scripts when in hitstun
        foreach (IScriptable s in scriptableScripts)
        {
            s.ScriptHandler(false);
        }
            
        
        //apply hitstop
        if (FXM != null)
        {
            FXM.StopTime(ad.hitstop / 100);
        }

        //flash white
        if (!NoFlash)
        {
            InvokeFlash(ad.hitstop * 0.005f, Color.white);
        }
        
    }
    public void InvokeFlash(float duration, Color c, bool solidColor = true, bool priority = true, int repeat = 1, float endingDuration = 0.05f)
    {
        //if something is already running and you're not a priority, return
        if (running != null && !priority) return;
        if (running != null) StopCoroutine(running);
          running = Flash(duration, c, solidColor, repeat, endingDuration);
        StartCoroutine(running);
    }
    public void CancelFlash()
    {
        if (running == null) return;
        StopCoroutine(running);
        for (int j = 0; j < sr.Length; j++)
        {
            if (sr[j].gameObject.tag == "Shadow") continue;
            sr[j].material.shader = OriginalShader[j];
            sr[j].material.color = Color.white;
            sr[j].color = OriginalColors[j];
        }
        running = null;
    }
    IEnumerator Flash(float duration, Color c, bool solidColor = true, int repeats = 1, float endingDuration = 0.05f)
    {
        for(int i = 0; i < repeats; i++)
        {
            //flash color
            for (int j = 0; j < sr.Length; j++)
            {
                if (sr[j].gameObject.tag == "Shadow") continue;
                if (solidColor)
                {
                    sr[j].material.shader = HitShader;
                    sr[j].material.color = c;
                    sr[j].color = OriginalColors[j];
                }
                else
                    sr[j].color = c;
            }
            //duration
            yield return new WaitForSeconds(duration);
            //unflash
            for (int j = 0; j < sr.Length; j++)
            {
                if (sr[j].gameObject.tag == "Shadow") continue;
                sr[j].material.shader = OriginalShader[j];
                sr[j].material.color = Color.white;
                sr[j].color = OriginalColors[j];
            }

            if(i != repeats - 1) yield return new WaitForSeconds(endingDuration);
        }
        running = null;
    }

    void Awake()
    {
        HitEffect = Resources.Load("Prefabs/Particles/HitEffect Particle") as GameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        scriptableScripts = gameObject.GetComponentsInChildren<IScriptable>();
        damagableScripts = gameObject.GetComponentsInChildren<IDamagable>();
        uniqueScript = gameObject?.GetComponent<IUnique>();
        animator = gameObject?.GetComponent<Animator>();
        sr = GetComponentsInChildren<SpriteRenderer>();
        FXM = GameObject.Find("EffectsManager")?.GetComponent<EffectsManager>();
        audiosrc = gameObject?.GetComponent<AudioSource>();
        hitstunTimer = -1;
        inHitStun = false;
        
        HitShader = Shader.Find("GUI/Text Shader");
        OriginalShader = new Shader[sr.Length];
        OriginalColors = new Color[sr.Length];
        for(int i = 0; i < sr.Length; i++)
        {
            OriginalShader[i] = sr[i].material.shader;
            OriginalColors[i] = sr[i].color;
        }  
    }
    public void HitstunTimer()
    {
        //timer to get yourself out of hitstun
        if (inHitStun)
            hitstunTimer -= Time.deltaTime;
        if (hitstunTimer <= 0 && inHitStun)
        {
            foreach (IScriptable s in scriptableScripts)
                s.ScriptHandler(true);
            inHitStun = false;
            //if(agent != null) agent.enabled = true;
            //set player's animator hitstun bool to false
            if (tag == "Player") animator.SetBool("Hitstun", false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!FSMHitstun)
        {
            HitstunTimer();
        }
    }
    
}
