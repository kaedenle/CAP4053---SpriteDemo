using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemyScript : MonoBehaviour, IUnique
{
    private Animator animator;
    private SpriteRenderer sr;
    private Rigidbody2D body;
    private ItemDrop drops;
    private AudioSource audiosrc;
    private float MAX_DEATH_TIMER = 2.5f;
    private float death_timer;
    public void EffectManager(string funct)
    {
    }
    public void onDeath()
    {
        animator.SetTrigger("Death");
        if (drops != null) drops.AttemptDrop();
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
        while (death_timer > 0)
        {
            while (EntityManager.IsPaused()) yield return null;
            Color c = sr.color;
            c.a = c.a > 0 ? c.a - 0.001f : 0;
            sr.color = c;
            death_timer -= Time.deltaTime;
            if (healthTracker.bar.gameObject != null && healthTracker.TotalDown() == 0) Destroy(healthTracker.bar.gameObject);
            //wait one frame
            yield return null;
        }
        yield return new WaitForSeconds(0.05F);
        
        Destroy(gameObject);
    }

    public void HitStunAni()
    {
        animator.Play("Hurt");
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        body = gameObject.GetComponent<Rigidbody2D>();
        drops = gameObject?.GetComponent<ItemDrop>();
        audiosrc = gameObject?.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
