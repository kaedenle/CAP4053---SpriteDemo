using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour, IDamagable
{
    public IDamagable[] damagableScripts;
    public IScriptable[] scriptableScripts;
    public IUnique uniqueScript;
    private float hitstunTimer;
    private Animator animator;
    public string default_ani;
    private bool inHitStun;
    public void damage(Vector3 knockback, int damage, float hitstun)
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
            
        }
            
        foreach (IScriptable s in scriptableScripts)
            s.ScriptHandler(false);
        
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
    }
    
}
