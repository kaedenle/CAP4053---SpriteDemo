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
    public void EffectManager(string script)
    {

    }
    public bool isAttacking()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Attack");
    }
    public void onDeath()
    {
        Destroy(ht.bar);
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
    }

    // Update is called once per frame
    void Update()
    {
        FlipEntity();
    }
}
