using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
List of Assumptions
- Camera is Orthographic
*/

// works in conjuction with BasicEnemy.FSM and MovementController to control basic enemy
public class EnemyBase : MonoBehaviour, IUnique, IDamagable
{    
    // automatically found GameObjects or Components
    protected Animator animator;
    protected Rigidbody2D body;
    protected SpriteRenderer sr;
    protected HealthTracker healthTracker; 
    protected MovementController movementController;
    protected Transform target;
    protected BasicEnemy.FSM fsm;

    // inherent configuration variables of the enemy
    public EnemyStats enemyStats;

    /* Awake, Start, Update */
    protected void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        body = gameObject.GetComponent<Rigidbody2D>();
        healthTracker = GetComponent<HealthTracker>();
        target = GeneralFunctions.GetPlayer().transform;
        movementController = gameObject.GetComponent<MovementController>();
        fsm = gameObject.GetComponent<BasicEnemy.FSM>();
    }
    
    protected void Update()
    {        
        //if((transform.position.x > target.position.x) ^ (dir < 0))
        //{
    //         TurnAround();
    //     }
    }

    /* 
    ================= Actions ================
    */
    public virtual void Idle()
    {
        // ??? do nothing
    }

    public virtual void Chase()
    {
        // movementController.MoveTowards(minimumDistance);
        transform.position = Vector2.MoveTowards(transform.position, target.position, enemyStats.speed * Time.deltaTime);
    }

    public virtual void ExpressSurprise(BasicEnemy.FSM stateMachine)
    {
        // have an exclamation mark pop up over enemy head & make surprise noise
        StartCoroutine( StopBeingSurprised(stateMachine) );
    }

    public IEnumerator StopBeingSurprised(BasicEnemy.FSM stateMachine)
    {
        yield return new WaitForSeconds(enemyStats.surprise_reaction_time);
        stateMachine.TransitionReady = true;
    }

    public void Attack(BasicEnemy.FSM stateMachine)
    {
        StartCoroutine(TriggerAttack(stateMachine));
    }

    public IEnumerator TriggerAttack(BasicEnemy.FSM stateMachine)
    {
        movementController.Attack();

        yield return new WaitUntil(() => movementController.enabled);
        stateMachine.TransitionReady = true;
    }

    public void Alerted(BasicEnemy.FSM stateMachine)
    {
        // question mark over enemy head
        // sound?
        StartCoroutine(CompleteTimer(enemyStats.memory_time, stateMachine));
    }

    public IEnumerator CompleteTimer(float time_wait, BasicEnemy.FSM stateMachine)
    {
        float start_time = Time.time;
        yield return new WaitForSeconds(time_wait);
        
        // if the state is still the same
        if(stateMachine.StateConstant(start_time))
        {
            stateMachine.TimerComplete = true;
        }
    }

    public void UpdateVision(MovementStats.FOVType visionType)
    {
        movementController.UpdateVision(visionType);
    }

    /* Conditions */
    public virtual bool PlayerVisibile()
    {
        return movementController.FOVCheck();
    }

    public virtual bool InRangeOfPlayer()
    {
        return movementController.InRangeOfPlayer(enemyStats.minimumDistance);
    }

    /* IUnique Functions */
    public virtual void onDeath()
    {
        // destory FSM (don't want to be doing things while dead)
        Destroy(fsm);

        // drop any attached items
        foreach (ItemDrop item in gameObject.GetComponents<ItemDrop>())
        {
            item.AttemptDrop();
        }

        // removing health tracker
        HealthTracker healthTracker = GetComponent<HealthTracker>();
        Destroy(healthTracker.bar.gameObject);

        // perform manager & metric updates on this enemy death
        PerformMetricsForDeath();

        // remove the enemy from the game
        Destroy(gameObject);
    }

    public virtual void HitStunAni()
    {
        
    }

    public virtual void EffectManager(string funct)
    {

    }

    /* IDamagable Functions */
    // deals with knockback
    public void damage(AttackData ad){
        if(body == null)
            return;

        body.AddForce(ad.knockback, ForceMode2D.Impulse);
    }

    /* Helper Functions */
    // metrics update for Kaeden's enemy death metric
    protected void PerformMetricsForDeath()
    {
        PlayerMetricsManager.IncrementKeeperInt("killed");
    }

    /*
    ======= Animation ========
    */

    public virtual void MoveAnimation() {}
}


/* 
FSM Planning

-> visibility distance based off of camera size

Idle, Wander
-> initial area of awareness is low (base angle + base distance) 
-> wander is based on presentation

Aware of Player (Transition)
-> choice of chase, flee, alert others
-> only alert in certain circumstances

Chase
-> base state after becoming aware

Engage
-> attack stage
-> disengage when health gets too low too quickly (alternatively, damange goes up if fighting in a blind rage)
-> speed is lowered when damaged greatly

Alert Others
-> if in a group and not engaged

Retreat
-> may be the best option if low on health and not get help
-> alternatively, low health actually makes enemy call for help

Alert
-> when see a fleeing mob
-> if nearby to a mob that alerts others

When hit -> yell out

*/