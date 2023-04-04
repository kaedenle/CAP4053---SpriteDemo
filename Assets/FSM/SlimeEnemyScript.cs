using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemyScript : MonoBehaviour, IUnique
{
    private Animator animator;
    private PlayerMetricsManager pmm;
    public void EffectManager(string funct)
    {
    }
    public void onDeath()
    {
        HealthTracker healthTracker = GetComponent<HealthTracker>();
        //Debug.Log("Dead Ooze boy");
        pmm.IncrementKeeperInt("killed");
        Destroy(healthTracker.bar.gameObject);
        Destroy(gameObject);
    }

    public void HitStunAni()
    {
        //TEMPORARY
        animator.Play("SlimeIdle");
    }
    void Awake()
    {
        pmm = GameObject.Find("PlayerMetricsManager").GetComponent<PlayerMetricsManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
