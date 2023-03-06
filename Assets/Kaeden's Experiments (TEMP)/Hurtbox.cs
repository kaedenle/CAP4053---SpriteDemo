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
        StopTime(hitstop/100);
    }

    public void StopTime(float duration)
    {
        if (waiting || Time.timeScale != 1)
            return;
        //change this if want to go slower rather than stop
        Time.timeScale = 0;
        StopCoroutine(Wait(duration));
        StartCoroutine(Wait(duration));
    }
    IEnumerator Wait(float amt)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(amt);
        //ResumeTime = true;
        Time.timeScale = 1f;
        waiting = false;
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

        if (ResumeTime)
        {
            if(Time.timeScale < 1f)
            {
                float time = Time.deltaTime == 0 ? 0.1f : Time.deltaTime * 10;
                Time.timeScale += time;
            }
            else
            {
                Time.timeScale = 1f;
                ResumeTime = false;
            }
        }

    }
    
}
