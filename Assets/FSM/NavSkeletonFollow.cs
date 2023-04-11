using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavSkeletonFollow : MonoBehaviour, IScriptable, IAI
{
    public float speed;
    public Transform target;
    public float minimumDistance;
    public float lineOfSightDistance;
    private HealthTracker healthTracker;
    private Animator animator;
    private SpriteRenderer sr;
    private AttackManager am;
    private Vector3 originalSpot;
    //gameObject enemy;
    NavMeshAgent agent;
    private NavMeshPath path;
    private bool attacking = false;

    private const float ATTACK_TIMER_MAX = 0.0f;
    private float attackTimer;

    // Update is called once per frame
    void Start()
    {
        originalSpot = gameObject.transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        healthTracker = GetComponent<HealthTracker>();
        target = GameObject.Find("Player").transform;
        am = GetComponent<AttackManager>();
        path = new NavMeshPath();
    }

    //enable and disable script
    public void ScriptHandler(bool flag)
    {
        if (flag)
        {
            attackTimer = ATTACK_TIMER_MAX;
            attacking = false;
        }
        else
        {
            if(agent.enabled) agent.SetDestination(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z));
        }
        this.enabled = flag;
        if(agent.enabled) agent.isStopped = !flag;
        //agent.enabled = flag;
        
    }
    void destroyEnemy()
    {
        //Destroy(enemy);
    }
    //what happens when disable
    public void EnableByID(int ID)
    {
        if (ID == 0)
            this.enabled = true;
        if(agent.enabled) agent.isStopped = false;
        //agent.enabled = true;
    }
    public void DisableByID(int ID)
    {
        if (ID == 0)
            this.enabled = false;
        if(agent.enabled) agent.isStopped = true;
        //agent.enabled = false;
    }
    void Update()
    {
        
        animator.SetFloat("movement", 0);
            //Debug.Log(healthTracker.healthSystem.getHealth());
            if (transform.position.x < target.position.x)
        {
            //sr.flipX = true;
            float newX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(newX, transform.localScale.y, transform.localScale.z);
            //Debug.Log("Flipping should happen");
        }
        else
        {
            float newX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(-newX, transform.localScale.y, transform.localScale.z);
            //sr.flipX = false;
        }
        if (Vector2.Distance(transform.position, target.position) > lineOfSightDistance)
        {
            transform.position = transform.position;
        }
        else
        {
            if (Vector2.Distance(transform.position, target.position) > minimumDistance)
            {
                if(agent.enabled)
                {
                    agent.CalculatePath(target.position, path);
                    if (path.status == NavMeshPathStatus.PathComplete && !attacking)
                    {
                        agent.isStopped = false;
                        //agent.updatePosition = true;
                        agent.SetDestination(new Vector3(target.position.x, target.position.y, transform.position.z));
                        //transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                        animator.SetFloat("movement", 1);
                    }
                    else
                    {
                        agent.isStopped = true;
                    }
                }
            }
            else
            {
                //attack here
                if(attackTimer >= 0) attackTimer -= Time.deltaTime;
                if (attackTimer < 0 && !attacking)
                {
                    attacking = true;
                    if(agent.enabled) agent.isStopped = true;
                    am.InvokeAttack("SlimeAttack");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Player")//or tag
        {
            //collision.GetComponent<HealthTracker>().damage(new Vector2(10,10), 5);
        }
    }
}
