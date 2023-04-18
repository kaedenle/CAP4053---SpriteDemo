using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using TMPro;


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
    protected GameObject expression;

    // inherent configuration variables of the enemy
    public EnemyStats enemyStats;
    private EnemyStats.SurpriseReactionType currentSurpriseReaction; 
    public Vector3 expressionOffset;
    

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

        // set default enemy configs
        currentSurpriseReaction = EnemyStats.SurpriseReactionType.Slow;
        movementController.UpdateVision(MovementStats.FOVType.Small);

    }

    protected void Start()
    {
        // create expression
        CreateExpression();
    }
    
    protected void Update()
    {        
        //if((transform.position.x > target.position.x) ^ (dir < 0))
        //{
    //         TurnAround();
    //     }
        // expression.GetComponent<FollowTarget>().offset = expressionOffset;
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
        // transform.position = Vector2.MoveTowards(transform.position, target.position, enemyStats.speed * Time.deltaTime);
        movementController.Chase();
    }

    public virtual void ExpressSurprise(BasicEnemy.FSM stateMachine)
    {
        // have an exclamation mark pop up over enemy head & make surprise noise
        StartCoroutine( StopBeingSurprised(stateMachine) );
    }

    public IEnumerator StopBeingSurprised(BasicEnemy.FSM stateMachine)
    {
        yield return new WaitForSeconds(enemyStats.GetSurpriseReactionTime(currentSurpriseReaction));
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
        
        // make sure shooting isn't occurring
        MoveAndShootController shooter;
        if((shooter = GetComponent<MoveAndShootController>()) != null)
        {
            yield return new WaitUntil(() => !shooter.shooting);
        }
        

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

    public void UpdateExpression(string str)
    {
        expression.GetComponentInChildren<TMP_Text>().text = str;
    }

    /* Stat Update Actions */
    public void UpdateVision(MovementStats.FOVType visionType)
    {
        movementController.UpdateVision(visionType);
    }

    public void UpdateSurpriseReactionTime(EnemyStats.SurpriseReactionType reactionType)
    {
        currentSurpriseReaction = reactionType;
    }

    /* Conditions */
    // check whether the player is visible and if so, updates the last seen position of the player
    public virtual bool PlayerVisibile()
    {
        return movementController.FOVCheck();
    }

    public virtual bool InRangeOfPlayer()
    {
        return movementController.InRangeOfPlayer();
    }

    // the enemy doesn't know how to continue chasing
    // either the enemy has reached the last known location of the player
    // OR the enemy hasn't seen the player for <memory> amount of time
    public bool NoIdeas()
    {
        if(PlayerVisibile()) return false; // has ideas if player is visible
        return movementController.AtLastSeen() || (movementController.TimeSinceLastSeen() >= enemyStats.memory_time);
    }

    /* IUnique Functions */
    public virtual void onDeath()
    {
        DestroyExtraComponents();

         // removing health tracker (skeleton has different behavior)
        HealthTracker healthTracker = GetComponent<HealthTracker>();
        Destroy(healthTracker.bar.gameObject);

        AttemptDrops();

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

    protected void CreateExpression()
    {
        GameObject Parent = GameObject.Find("UI Canvas");
        if(Parent == null)
        {
            Debug.LogError("no UI canvas found to put enemy expression under");
            return;
        }

        GameObject express = Resources.Load("Prefabs/Expression") as GameObject;

        FollowTarget info = express.GetComponent<FollowTarget>();
        info.target = this.gameObject;
        info.offset = expressionOffset;

        expression = Instantiate(express) as GameObject;
        expression.transform.SetParent(Parent.transform);
        expression.name = "Enemy Expression";

        // default expression is none
        UpdateExpression("");
    }

    protected void AttemptDrops()
    {
        // drop any attached items
        foreach (ItemDrop item in gameObject.GetComponents<ItemDrop>())
        {
            item.AttemptDrop();
        }
    }

    protected void DestroyExtraComponents()
    {
        // destory FSM (don't want to be doing things while dead)
        Destroy(fsm);
        // removing expression
        Destroy(expression);
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