using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CapoScript : MonoBehaviour, IUnique
{
    private EffectsManager fxm;
    private Rigidbody2D body;
    private NavMeshAgent agent;
    private Animator anim;
    private Hurtbox hb;
    private AttackManager am;
    private HealthTracker ht;
    public bool flipped;
    public bool kick;
    public int SpawnID;
    private float death_timer;
    private float MAX_DEATH_TIMER = 3.0f;
    private SpriteRenderer sr;
    public void EffectManager(string script)
    {

    }
    public bool isAttacking()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Attack");
    }
    public void onDeath()
    {
        anim.SetBool("Death", true);
        anim.Play("Death");
    }
    public void AnimationDeath()
    {
        StartCoroutine("Disappear");
        agent.isStopped = true;
    }
    public IEnumerator Disappear()
    {
        death_timer = MAX_DEATH_TIMER;
        HealthTracker healthTracker = GetComponent<HealthTracker>();
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
    public void HitStunAni()
    {
        agent.isStopped = true;
        am.DestroyPlay();
        anim.Play("Hurt");
        anim.SetBool("Hitstun", true);
    }
    public void LeaveHitStun()
    {
        anim.SetBool("Hitstun", false);
    }
    
    public void Shake(float magnitude)
    {
        fxm.ShakeCam(0.5f, magnitude);
    }
    public void AddForceX(float magnitude)
    {
        if (flipped) magnitude *= -1;
        body.AddForce(new Vector2(magnitude, 0), ForceMode2D.Impulse);
    }
    private void FlipEntity()
    {
        float newX = Mathf.Abs(transform.localScale.x);
        if (flipped) transform.localScale = new Vector3(-newX, transform.localScale.y, transform.localScale.z);
        else transform.localScale = new Vector3(newX, transform.localScale.y, transform.localScale.z);
    }
    // Start is called before the first frame update
    void Start()
    {
        fxm = GameObject.Find("EffectsManager").GetComponent<EffectsManager>();
        body = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        hb = GetComponent<Hurtbox>();
        am = GetComponent<AttackManager>();
        ht = GetComponent<HealthTracker>();
        sr = GetComponent <SpriteRenderer> ();
    }

    // Update is called once per frame
    void Update()
    {
        FlipEntity();
    }
}
