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
    private bool inHitStun;

    //hitstop variables
    public string default_ani;

    //misc variables
    private Animator animator;
    //CHANGE THIS TO ARRAY (for children detection)
    private SpriteRenderer sr;
    private HitStopManager HSM; //used for hitstop
    public void damage(Vector3 knockback, int damage, float hitstun, float hitstop)
    {
        hitstunTimer = hitstun;
        inHitStun = true;
        //CHANGE LOGIC HERE TO ROUTE TO HITSTUN ANIMATION
        if(uniqueScript == null)
        {
            if (animator != null) animator.Play(default_ani);
        }
        else
        {
            uniqueScript.HitStunAni();
        }
        AttackManager am = this?.GetComponent<AttackManager>();
        if (am != null)
        {
            am.DestroyPlay();
            Debug.Log(name + " here");
        }
            

        foreach (IScriptable s in scriptableScripts)
            s.ScriptHandler(false);

        //apply hitstop
        if (HSM != null)
        {
            //apply sr here piece by piece
            HSM.StopTime(hitstop / 100);
        }
    }

    
    
    // Start is called before the first frame update
    void Start()
    {
        scriptableScripts = gameObject.GetComponentsInChildren<IScriptable>();
        damagableScripts = gameObject.GetComponentsInChildren<IDamagable>();
        uniqueScript = this?.GetComponent<IUnique>();
        hitstunTimer = -1;
        animator = this?.GetComponent<Animator>();
        sr = this?.GetComponent<SpriteRenderer>();
        inHitStun = false;
        HSM = GameObject.Find("HitStopManager")?.GetComponent<HitStopManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(inHitStun) 
            hitstunTimer -= Time.deltaTime;
        if (hitstunTimer <= 0 && inHitStun)
        {
            foreach (IScriptable s in scriptableScripts)
                s.ScriptHandler(true);
            inHitStun = false;
        }

    }
    
}
