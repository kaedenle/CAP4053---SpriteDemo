using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IUnique
{
    // Start is called before the first frame update
    private Animator animator;
    private Rigidbody2D body;
    private bool killPlayer = false;
    private float DeathDelayTimer;
    private float MAX_DEATH_TIMER = 1.5f;
    public int MaxAmmo;
    private int Ammo;
    private AttackManager am;
    private WeaponManager wm;
    
    public void EffectManager(string funct)
    {
        //call function via string reference
        Invoke(funct, 0f);
    }

    public void Shoot()
    {
        am.ScriptActivate(AttackManager.ScriptTypes.Movement);
        if (Ammo > 0 && animator.GetFloat("shooting") == 0)
        {
            wm.equipInput = false;
            wm.swapInput = false;
            animator.SetFloat("shooting", 1);
            Ammo -= 1;
            EntityManager.DisableAttack();
        }
        //animator.SetFloat("attack", 0);
    }

    public void UnShoot()
    {
        if(Ammo > 0)
        {
            wm.equipInput = true;
            wm.swapInput = true;
            animator.SetFloat("shooting", 0);
            EntityManager.EnableAttack();
        }
        else
        {
            //disable movement and set to reload
            animator.SetFloat("shooting", 2);
            animator.Play("Buffer", 1);
            am.ScriptDeactivate(AttackManager.ScriptTypes.Movement);
        }
        
    }
    //set variables associated with reload animation
    public void CleanShoot()
    {
        am.ScriptActivate(AttackManager.ScriptTypes.Movement);
        wm.equipInput = true;
        wm.swapInput = true;
        animator.SetFloat("shooting", 0);
        animator.Play("Idle", 1);
        Ammo = MaxAmmo;
        EntityManager.EnableAttack();
    }

    private void Sword1()
    {
        bool flipped = animator.GetBool("flipped");
        Vector3 move = new Vector3(30, 0, 0);
        if (flipped) move *= -1;
        body.AddForce(move, ForceMode2D.Impulse);
    }

    public void onDeath()
    {
        Debug.Log("You've died!");

        //disable all scripts
        Hurtbox hrt = gameObject?.GetComponent<Hurtbox>();
        IScriptable[] list = hrt != null ? hrt.scriptableScripts : GetComponents<IScriptable>();
        foreach (IScriptable script in list)
            script.ScriptHandler(false);

        //play death animation
        animator.Play("Death");
    }

    //AnimatorOnly Function called when death animation finishes
    public void AnimatorPlayerDeath()
    {
        //set flag to freeze game and pull up Death UI
        killPlayer = true;
        DeathDelayTimer = MAX_DEATH_TIMER;
    }

    public void HitStunAni()
    {
        //if current clip is death don't play below
        //GetComponent<AttackManager>().DestroyPlay();
        //TEMPORARY

        bool equiped = animator.GetBool("equiped");
        if (equiped)
            animator.Play("Idle_E");
        else
            animator.Play("Idle");
    }

    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        wm = gameObject.GetComponent<WeaponManager>();
        am = gameObject.GetComponent<AttackManager>();
        Ammo = MaxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        //play death animation when dead if haven't already (fixes bug that plays idle_engage before death can start playing)
        if ((animator.GetCurrentAnimatorClipInfo(0).Length > 0 && animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Death") && GetComponent<HealthTracker>().healthSystem.getHealth() <= 0)
            onDeath();

        //if flag is true and HP bar is all the way down
        if (killPlayer && GetComponent<HealthTracker>().GetTrueFillAmount() <= 0)
        {
            if(DeathDelayTimer > 0)
                DeathDelayTimer -= Time.deltaTime;
            if(DeathDelayTimer <= 0)
            {
                EntityManager.PlayerDied();
                killPlayer = false;
            }
                
        }
            
    }

    void LateUpdate()
    {
        bool flipped = animator.GetBool("flipped");
        int temp = flipped ? 180 : 0;

        //update the Weapon tag to rotate correctly (all will be animated the same)
        GameObject[] wpnObjs = GameObject.FindGameObjectsWithTag("Weapon");
        foreach (GameObject wpn in wpnObjs)
        {
            GameObject parent = wpn.transform.parent.gameObject;
            //flip and flip to default
            if (wpn.transform.rotation.eulerAngles.y < 180 && flipped)
                wpn.transform.RotateAround(parent.transform.position, Vector3.up, 180);
            else if (wpn.transform.rotation.eulerAngles.y >= 180 && !flipped)
                wpn.transform.RotateAround(parent.transform.position, Vector3.up, 0);
        }
    }
}
