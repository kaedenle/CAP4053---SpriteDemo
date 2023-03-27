using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour, IDamagable
{
    //reference variables
    public IDamagable[] damagableScripts;
    public IScriptable[] scriptableScripts;
    public IUnique uniqueScript;

    //hitstun variables
    private float hitstunTimer;
    public bool inHitStun;
    public bool NotFlash;

    //hitstop variables
    public string default_ani;
    private GameObject HitEffect;
    private EffectsManager FXM; //used for hitstop
    private Shader HitShader;
    private Shader OriginalShader;

    //misc variables
    private Animator animator;
    private bool FlashFlag = false;
    private float FlashTimer;
    private SpriteRenderer[] sr;
    public void damage(Vector3 knockback, int damage, float hitstun, float hitstop, int weapon)
    {
        //Hit particle effect
        if (HitEffect != null)
        {
            GameObject instance = Instantiate(HitEffect, transform.position, Quaternion.identity);
            instance.transform.SetParent(gameObject.transform);
            //set correct scale
            float XScale = instance.transform.localScale.x * gameObject.transform.localScale.x + (0.05f*hitstop);
            float YScale = instance.transform.localScale.y * gameObject.transform.localScale.y + (0.05f*hitstop);
            instance.transform.localScale = new Vector3(XScale, YScale, instance.transform.localScale.z);

            //play slower if hitstop is greater
            ParticleSystem ps = instance.GetComponent<ParticleSystem>();
            var main = ps.main;
            float Speed = main.simulationSpeed - hitstop * 0.02f > 0 ? main.simulationSpeed - hitstop * 0.02f: 0.001f;
            main.simulationSpeed = Speed;
        }

        //if not dead play hitstun animation
        HealthTracker ht = gameObject?.GetComponent<HealthTracker>();
        if (ht != null && ht.healthSystem.getHealth() > 0)
        {
            hitstunTimer = hitstun;
            inHitStun = true;
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
            FXM.StopTime(hitstop / 100);
        }

        //flash white
        if (!NotFlash)
        {
            foreach (SpriteRenderer piece in sr)
            {
                piece.material.shader = HitShader;
                piece.material.color = Color.white;

            }
            FlashTimer = hitstop * 0.005f;
            FlashFlag = true;
        }
        
    }
    IEnumerator Wait(float amt)
    {
        yield return new WaitForSecondsRealtime(amt);
        FlashFlag = false;
        //unflash white
        foreach (SpriteRenderer piece in sr)
        {
            piece.material.shader = OriginalShader;
        }
    }

    void Awake()
    {
        HitEffect = Resources.Load("Prefabs/HitEffect Particle") as GameObject;
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

        hitstunTimer = -1;
        inHitStun = false;
        
        HitShader = Shader.Find("GUI/Text Shader");
        OriginalShader = sr.Length > 0 ? sr[0].material.shader : null;
    }

    // Update is called once per frame
    void Update()
    {
        //timer to get yourself out of hitstun
        if(inHitStun) 
            hitstunTimer -= Time.deltaTime;
        if (hitstunTimer <= 0 && inHitStun)
        {
            foreach (IScriptable s in scriptableScripts)
                s.ScriptHandler(true);
            inHitStun = false;
        }

        if (FlashFlag)
            FlashTimer -= Time.deltaTime;
        //unflash white after unpause
        if (FlashTimer <= 0 && FlashFlag)
        {
            foreach (SpriteRenderer piece in sr)
                piece.material.shader = OriginalShader;
            FlashFlag = false;
        }

    }
    
}
