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
    private bool ResumeTime = false;
    private bool waiting = false;

    //misc variables
    private Animator animator;
    private EntityManager EM; //used for hitstop
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
            AttackManager am = this?.GetComponent<AttackManager>();
            if (am != null) am.DestroyPlay();
            uniqueScript.HitStunAni();
        }
            
        foreach (IScriptable s in scriptableScripts)
            s.ScriptHandler(false);

        //apply hitstop
        EM.StopTime(hitstop/100);
    }

    
    
    // Start is called before the first frame update
    void Start()
    {
        scriptableScripts = gameObject.GetComponentsInChildren<IScriptable>();
        damagableScripts = gameObject.GetComponentsInChildren<IDamagable>();
        uniqueScript = this?.GetComponent<IUnique>();
        hitstunTimer = -1;
        animator = this?.GetComponent<Animator>();
        inHitStun = false;
        EM = GameObject.Find("EntityManager").GetComponent<EntityManager>();
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
