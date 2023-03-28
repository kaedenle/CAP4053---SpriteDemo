using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemyScript : MonoBehaviour, IUnique
{
    private Animator animator;
    private SpriteRenderer sr;
    private Rigidbody2D body;
    private float MAX_DEATH_TIMER = 2.5f;
    private float death_timer;
    public void EffectManager(string funct)
    {
    }
    public void onDeath()
    {
        animator.SetTrigger("Death");
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
            //wait one frame
            yield return null;
        }
        yield return new WaitForSeconds(0.05F);
        HealthTracker healthTracker = GetComponent<HealthTracker>();
        Destroy(healthTracker.bar.gameObject);
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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
