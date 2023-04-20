using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonBase : EnemyBase
{    
    public bool shooting = false;

    private AudioSource audiosrc;
    private Hurtbox hrt;
    private float MAX_DEATH_TIMER = 2.5f;
    private float death_timer;

    public override void onDeath()
    {
        // same as parent
        SpecialCaseDeaths();
        DestroyExtraComponents();
        AttemptDrops();
        PerformMetricsForDeath();

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        //if (agent != null) agent.enabled = false;
        if(agent != null)
        {
            agent.enabled = false;
        }
        
        animator.SetBool("Death", true);

        IScriptable[] scripts = GetComponents<IScriptable>();
        foreach (IScriptable uni in scripts)
            uni.ScriptHandler(false);
        
        body.velocity = Vector2.zero;

        foreach (BoxCollider2D box in transform.GetComponentsInChildren<BoxCollider2D>())
            box.enabled = false;
    }
    
    public void AnimationDeath()
    {
        StartCoroutine("Disappear");
    }
    
    public IEnumerator Disappear()
    {
        death_timer = MAX_DEATH_TIMER;

        while (death_timer > 0)
        {
            while (EntityManager.IsPaused()) yield return null;
            Color c = sr.color;
            c.a = c.a > 0 ? c.a - 0.005f : 0;
            sr.color = c;
            death_timer -= Time.deltaTime;
            if (healthTracker.bar.gameObject != null && healthTracker.TotalDown() == 0) Destroy(healthTracker.bar.gameObject);
            //wait one frame
            yield return null;
        }
        yield return new WaitForSeconds(0.05F);
        if (healthTracker.bar.gameObject != null) Destroy(healthTracker.bar);
        Destroy(gameObject);
    }

    public override void HitStunAni()
    {
        animator.Play("Hurt");
        animator.SetBool("Hitstun", true);
        shooting = false;
    }
    // Start is called before the first frame update
    new void Awake()
    {
        audiosrc = gameObject?.GetComponent<AudioSource>();
        hrt = gameObject.GetComponent<Hurtbox>();
        base.Awake();
    }

    // Update is called once per frame
    new void Update()
    {
        //if hurt is null immediate set Hitstun to false
        //OR if not in hitstun from hurtbox, set animator value to false
        if ((hrt == null) || (hrt != null && !hrt.inHitStun))
            animator.SetBool("Hitstun", false);

        base.Update();
    }

    /*
    =============== Animation Overrides =================
    */

    public override void Chase()
    {
        animator.SetFloat("movement", 1);
        base.Chase();
    }

    public override void Idle()
    {
        animator.SetFloat("movement", 0);
        base.Idle();
    }
}
