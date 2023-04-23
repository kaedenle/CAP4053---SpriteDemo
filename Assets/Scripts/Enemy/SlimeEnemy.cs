using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeEnemy : EnemyBase
{
    protected const string idleAnimateLable = "SlimeIdle";
    private CameraShake cs;
    private AudioManager aud;
    private float MAX_DEATH_TIMER = 2.5f;
    private float death_timer;
    public void Shake()
    {
        cs.StartShake(0.5f);
    }
    new void Awake()
    {
        base.Awake();
        cs = Camera.main.GetComponent<CameraShake>();
        aud = GetComponent<AudioManager>();
        // expressionOffset = new Vector3(0.2F, 2.35F, 0);
    }
    public override void onDeath()
    {
        SpecialCaseDeaths();
        AttemptDrops();

        // perform manager & metric updates on this enemy death
        PerformMetricsForDeath();
        DestroyExtraComponents();
        GetComponent<NavMeshAgent>().enabled = false;
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
        HealthTracker healthTracker = GetComponent<HealthTracker>();
        if (healthTracker.bar.gameObject != null) Destroy(healthTracker.bar);
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
        Destroy(gameObject);
    }
    public override void HitStunAni()
    {
        //TEMPORARY
        animator.Play(idleAnimateLable);
        if(aud != null) aud.PlayAudio(1);
        base.HitStunAni();
    }
}
