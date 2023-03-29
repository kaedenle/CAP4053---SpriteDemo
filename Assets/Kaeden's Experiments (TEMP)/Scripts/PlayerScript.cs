using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IUnique
{
    //Object References
    private Animator animator;
    private Rigidbody2D body;
    private AttackManager am;
    private WeaponManager wm;
    private EffectsManager fxm;
    private Player_Movement pm;
    public static GameObject OnePlayer;
    private GameManager gm;

    //Death Handling
    private bool killPlayer = false;
    private float DeathDelayTimer;
    private float MAX_DEATH_TIMER = 1.5f;

    //Projectile Variables
    public int MaxAmmo;
    public GameObject bullet;
    private int Ammo;
    //cancel: "did player press?"
    //ShootAgain: "is my window open?"
    [HideInInspector]
    public bool cancel = false;
    [HideInInspector]
    public bool ShootAgain = false;

    //Variables to handle bug that sees you locked in place (very scuffed)
    private bool ShootEnd = true;
    private float ShootEndTimer;
    private float MAX_SHOOT_TIMER = 10f;

    public int TackleSpeed;

    public void EffectManager(string funct)
    {
        //call function via string reference
        if (funct == "") return;
        Invoke(funct, 0f);
    }
    public void StartShoot()
    {
        am.ScriptActivate(AttackManager.ScriptTypes.Movement);
        if (Ammo > 0 && animator.GetFloat("shooting") == 0)
        {
            EntityManager.DisableEquip();
            EntityManager.DisableSwap();
            animator.SetFloat("shooting", 1);
            ShootAgain = false;
            cancel = false;
        }
        //animator.SetFloat("attack", 0);
    }
    public void Shoot()
    {
        if (Ammo > 0)
        {
            Ammo -= 1;
            int turn = animator.GetBool("flipped") ? 1 : -1;
            GameObject proj = Instantiate(bullet, transform.Find("Right Arm").Find("On-Hand").transform.position - new Vector3((-1 * turn), 0, 0), Quaternion.identity);
            Bullet bulScript = proj?.GetComponent<Bullet>();
            if (bulScript != null) bulScript.InitBullet(gameObject);
        }
    }
    public void UnShoot()
    {
        if (Ammo > 0)
        {
            EntityManager.EnableSwap();
            EntityManager.EnableEquip();
            animator.SetFloat("shooting", 0);
        }
        else
        {
            //disable movement and set to reload
            am.ScriptDeactivate(AttackManager.ScriptTypes.Movement);
            animator.SetFloat("shooting", 2);
            animator.Play("Buffer", 1);

            //just in case rare bug happens
            ShootEnd = false;
            ShootEndTimer = MAX_SHOOT_TIMER;
        }
        
    }
    public void Immobilize()
    {
        am.ScriptDeactivate(AttackManager.ScriptTypes.Movement);
    }
    //set variables associated with reload animation
    public void CleanShoot()
    {
        am.ScriptActivate(AttackManager.ScriptTypes.Movement);
        EntityManager.EnableSwap();
        EntityManager.EnableEquip();
        animator.SetFloat("shooting", 0);
        animator.Play("Idle", 1);
        Ammo = MaxAmmo;
        ShootEnd = true;
        cancel = false;
    }

    public void CancelShoot()
    {
        ShootAgain = true;
    }

    private void Sword1()
    {
        bool flipped = animator.GetBool("flipped");
        Vector3 move = new Vector3(30, 0, 0);
        if (flipped) move *= -1;
        body.AddForce(move, ForceMode2D.Impulse);
    }
    private void GunShake()
    {
        if (fxm != null) fxm.ShakeCam(0.1f, 0.75f);
    }
    private void QuakeShake()
    {
        if (fxm != null) fxm.ShakeCam(0.1f, 0.75f);
    }
    private void Tackle()
    {
        Vector3 temp = pm.direction();
        //bool flipped = animator.GetBool("flipped");
        Vector3 move = temp.normalized * TackleSpeed;
        //if (flipped) move *= -1;
        body.AddForce(move);
    }

    public void onDeath()
    {

        //Debug.Log("You've died!");

        //disable all scripts
        Hurtbox hrt = gameObject?.GetComponent<Hurtbox>();
        IScriptable[] list = hrt != null ? hrt.scriptableScripts : GetComponents<IScriptable>();
        //reset projectile variables
        animator.SetLayerWeight(1, 0);
        animator.SetFloat("shooting", 0);
        //disable all your inputs
        EntityManager.DisableSwap();
        EntityManager.DisableEquip();
        EntityManager.DisableAttack();

        //disable own collider on Body
        transform.Find("Body").GetComponent<BoxCollider2D>().enabled = false;

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

        //getting hit while reloading (reload for you then transition)
        if(animator.GetFloat("shooting") == 2) CleanShoot();
    }

    void Awake()
    {    
    }
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        wm = gameObject.GetComponent<WeaponManager>();
        am = gameObject.GetComponent<AttackManager>();
        Ammo = MaxAmmo;
        fxm = FindObjectOfType<EffectsManager>();
        pm = gameObject.GetComponent<Player_Movement>();
        GameObject gmo = GameObject.Find("GameManager");
        gm = gmo.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

        //fix bug where shoot value is active but weapon is not gun
        if (animator.GetFloat("shooting") > 0 && wm.wpnList.index != 2)
        {
            UnShoot();
        }
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
                gm.SoftResetManager();
                killPlayer = false;
            }
                
        }

        //This code fixes a rare bug that locks player in place after shooting
        if (!ShootEnd && ShootEndTimer > 0) ShootEndTimer -= Time.deltaTime;
        if(!ShootEnd && ShootEndTimer <= 0)
        {
            CleanShoot();
        }

        //if detect cancel
        if(cancel && ShootAgain)
        {
            cancel = false;
            ShootAgain = false;
            UnShoot();
            am.InvokeAttack(5);
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
