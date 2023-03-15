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

    //hitstop variables
    public string default_ani;

    //misc variables
    private Animator animator;
    private bool FlashFlag = false;
    private float FlashTimer;

    //CHANGE THIS TO ARRAY (for children detection)
    private SpriteRenderer[] sr;
    private HitStopManager HSM; //used for hitstop
    private Shader HitShader;
    private Shader OriginalShader;
    public void damage(Vector3 knockback, int damage, float hitstun, float hitstop)
    {
        
        //if not dead play hitstun animation
        if(GetComponent<HealthTracker>().healthSystem.getHealth() > 0)
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
        if (HSM != null)
        {
            HSM.StopTime(hitstop / 100);
        }

        //flash white
        foreach (SpriteRenderer piece in sr)
        {
            piece.material.shader = HitShader;
            piece.material.color = Color.white;
             
        }
        FlashTimer = hitstop * 0.005f;
        FlashFlag = true;
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

    // Start is called before the first frame update
    void Start()
    {
        scriptableScripts = gameObject.GetComponentsInChildren<IScriptable>();
        damagableScripts = gameObject.GetComponentsInChildren<IDamagable>();
        uniqueScript = this?.GetComponent<IUnique>();
        animator = this?.GetComponent<Animator>();
        sr = GetComponentsInChildren<SpriteRenderer>();
        HSM = GameObject.Find("HitStopManager")?.GetComponent<HitStopManager>();

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
