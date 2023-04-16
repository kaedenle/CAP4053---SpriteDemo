using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
List of Assumptions
- Camera is Orthographic
*/

public class EnemyBase : MonoBehaviour, IUnique, IDamagable
{
    // variables to be configured at creation
    public BasicEnemy.FSM fsm;
    public float speed;
    [Range(0, 100)] public float maxSightAsCamWidthPercent;
    [Range(0, 180)] public float maxBaseSightAngle;
    public float minimumDistance;
    
    // automatically found GameObjects or Components
    protected Animator animator;
    protected Rigidbody2D body;
    protected SpriteRenderer sr;
    protected HealthTracker healthTracker; 
    protected MovementController movementController;
    protected Transform target;

    // inherent configuration variables of the enemy
    protected float lineOfSightDistance;
    protected float surprise_reaction_time = 1.0F;
    protected float memory_time = 1.0F;

    // variables that keep track of the enemy's state
    private int dir = 1;

    /* Awake, Start, Update */
    protected void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        body = gameObject.GetComponent<Rigidbody2D>();
        healthTracker = GetComponent<HealthTracker>();
        target = GeneralFunctions.GetPlayer().transform;
        movementController = gameObject.GetComponent<MovementController>();
        
        // get starting direction (-1 or 1)
        dir = UnityEngine.Random.Range(0, 2);
        // is facing 
        if(dir == 0)
        {
            dir--;
        }

        else
        {
            TurnAround();
            dir = 1;
        }
    }
    
    
    protected void Start()
    {
        lineOfSightDistance = Camera.main.orthographicSize * 2f * Camera.main.aspect * maxSightAsCamWidthPercent / 100.0F;
        Debug.Log("baseMaxSightDistance=" + lineOfSightDistance);

    }

    protected void Update()
    {
        // fsm.ExecuteCurrentState();
        // if(!SeesPlayer(lineOfSightDistance, maxBaseSightAngle))
        // {
        //     transform.position = transform.position;
        // }
        // else
        // {
        //     if((transform.position.x > target.position.x) ^ (dir < 0))
        //     {
        //         TurnAround();
        //     }

        //     // move towards player
        //     if(Vector2.Distance(transform.position, target.position) > minimumDistance)
        //     {
        //         Chase();
        //     }

        //     else
        //     {   
        //         Attack();   
        //     }
        // }
    }

    /* Actions */
    public virtual void Idle()
    {
        // ??? do nothing
    }

    public virtual void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public virtual void ExpressSurprise(BasicEnemy.FSM stateMachine)
    {
        // have an exclamation mark pop up over enemy head & make surprise noise
        StartCoroutine( StopBeingSurprised(stateMachine) );
    }

    public IEnumerator StopBeingSurprised(BasicEnemy.FSM stateMachine)
    {
        yield return new WaitForSeconds(surprise_reaction_time);
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
        StartCoroutine(CompleteTimer(memory_time, stateMachine));
    }

    public IEnumerator CompleteTimer(float time_wait, BasicEnemy.FSM stateMachine)
    {
        yield return new WaitForSeconds(time_wait);
        stateMachine.TimerComplete = true;
    }

    /* Conditions */
    public virtual bool PlayerVisibile()
    {
        return SeesPlayer(lineOfSightDistance, maxBaseSightAngle);
    }

    public virtual bool InRangeOfPlayer()
    {
        return Vector2.Distance(transform.position, target.position) <= minimumDistance;
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

    // determine if the player is within the enemy's sight line
    // Potential bug: assumes all z values are valid
    protected bool SeesPlayer(float maxDistance, double maxAngleInDegrees)
    {
        // determine if the distance is valid
        float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);
        if(distance > maxDistance) return false;

        // determine if the sight angle is valid
        Vector3 baseSight = gameObject.transform.position - new Vector3(transform.position.x + (maxDistance * dir), transform.position.y, transform.position.z);
        Vector3 vecToPlayer = gameObject.transform.position - target.transform.position;
        double angle = Vector3.Angle(baseSight, vecToPlayer);
        // Debug.Log("angle is currently " + angle + " and target angle is " + maxAngleInDegrees);

        if(Math.Abs(angle) > maxAngleInDegrees)
            return false;
        
        return true;
    }

    // assumes that start is left
    protected virtual void TurnAround()
    {
        gameObject.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        dir = -dir;
    }
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