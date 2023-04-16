using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour, IScriptable, IAI
{

    public float radius;
    [Range(1.0F, 360.0F)]
    public float angle;
    public Transform target;
    public float minimumDistance;
    public float MAX_REMEMBER_TIMER = 3f;
    private AttackManager am;
    public float distFromPoint;

    //gameObject enemy;
    NavMeshAgent agent;
    private NavMeshPath path;
    private NavMeshPath pathToPoint;
    private int pointIndex;
    private bool attacking = false;
    private List<Collider2D> collidersList = new List<Collider2D>();
    private Vector3 looking;
    private bool flipLook;
    private Vector3 lastPos;
    private float rememberTimer = 0;

    private const float ATTACK_TIMER_MAX = 0.0f;
    private float attackTimer;
    private bool seesPlayer = false;
    private bool trigger = false;
    private Animator anim;
    //0 = up; 1 = right; 2 = down; 3 = left
    private int direction;

    public GameObject[] points;

    // Update is called once per frame
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        target = GameObject.Find("Player").transform;
        //am = GetComponent<AttackManager>();
        path = new NavMeshPath();
        pathToPoint = new NavMeshPath();
        LookingForDirection();
        lastPos = transform.position;
        anim = GetComponentInChildren<Animator>();
    }
    private float Round2Digits(float num)
    {
        return Mathf.Round(num * 10.0f) * 0.1f;
    }
    private void GetDirection()
    {
        float deltax = Mathf.Abs(lastPos.x - transform.position.x);
        float deltay = Mathf.Abs(lastPos.y - transform.position.y);

        if (deltay > deltax)
        {
            if (lastPos.y < transform.position.y) direction = 0;
            if (lastPos.y > transform.position.y) direction = 2;
        }
        if (Round2Digits(lastPos.x) < Round2Digits(transform.position.x))
        {
            //sr.flipX = true;
            float newX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(newX, transform.localScale.y, transform.localScale.z);
            if (deltax > deltay) direction = 1;
        }
        else if (Round2Digits(lastPos.x) > Round2Digits(transform.position.x))
        {
            float newX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(-newX, transform.localScale.y, transform.localScale.z);
            if (deltax > deltay) direction = 3;
        }
        lastPos = transform.position;
    }
    private void FlipEntity()
    {
        //right way
        if(target.transform.position.x > transform.position.x)
        {
            float newX = Mathf.Abs(transform.localScale.x);
           transform.localScale = new Vector3(newX, transform.localScale.y, transform.localScale.z);
        }
        //left way
        else if(target.transform.position.x < transform.position.x)
        {
            float newX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(-newX, transform.localScale.y, transform.localScale.z);
        }
    }
    private void LookingForDirection()
    {
        GetDirection();
        if (direction == 1) looking = Vector3.right;
        else if (direction == 3) looking = Vector3.left;
        else if (direction == 2) looking = Vector3.down;
        else if (direction == 0) looking = Vector3.up;
    }
    private bool FOVCheck()
    {
        LookingForDirection();
        var contactFilter = new ContactFilter2D();
        bool ret = false;
        //can only hit on entity layer (may change)
        contactFilter.layerMask = LayerMask.GetMask("Entity") | LayerMask.GetMask("Action");
        contactFilter.useLayerMask = true;

        collidersList.Clear();
        Physics2D.OverlapCircle(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y),
                radius, contactFilter, collidersList);

        if (collidersList.Count != 0)
        {
            Transform target = collidersList[0].transform;
            //find player
            foreach (Collider2D coll in collidersList)
            {
                if (coll.gameObject.tag == "Player")
                {
                    target = coll.gameObject.transform;
                    break;
                }
            }

            if (target.tag != "Player") return false;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector2.Angle(looking, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, contactFilter.layerMask);
                if (hit.collider != null)
                {
                    ret = true;
                }
            }
        }
        return ret;
    }
    //how long the entity remembers the player
    private void Memory()
    {
        if (rememberTimer >= 0) rememberTimer -= Time.deltaTime;
        if (rememberTimer < 0)
        {
            seesPlayer = false;
            agent.speed = 3.5f;
        }
    }

    private void MoveTowards()
    {
        //animator.SetFloat("movement", 0);
        //Debug.Log(healthTracker.healthSystem.getHealth());
        //FlipEntity();

        if (Vector2.Distance(transform.position, target.position) <= 1000f)
        {
            if (Vector2.Distance(transform.position, target.position) > minimumDistance)
            {
                if (agent.enabled)
                {
                    agent.CalculatePath(target.position, path);
                    if (path.status == NavMeshPathStatus.PathComplete && !attacking)
                    {
                        anim.SetFloat("movement", 2);
                        agent.isStopped = false;
                        //agent.updatePosition = true;
                        agent.SetDestination(new Vector3(target.position.x, target.position.y, target.position.z));
                        rememberTimer = MAX_REMEMBER_TIMER;
                        //transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                        //animator.SetFloat("movement", 1);
                    }
                    else
                    {
                        //agent.isStopped = true;
                        if (Mathf.Abs(lastPos.x - transform.position.x) == 0 && agent.velocity.x == 0 &&  agent.velocity.y == 0)
                            anim.SetFloat("movement", 0);
                    }
                }
            }
            else
            {
                agent.isStopped = true;
                anim.SetFloat("movement", 0);
                rememberTimer = MAX_REMEMBER_TIMER;
                //attack here
                /*if (attackTimer >= 0) attackTimer -= Time.deltaTime;
                if (attackTimer < 0 && !attacking)
                {
                    //attacking = true;
                    if (agent.enabled) agent.isStopped = true;
                    //am.InvokeAttack("SlimeAttack");
                }*/
            }
        }
    }
    private void Patroling()
    {
        if (points.Length == 0 || !agent.enabled) return;
        agent.CalculatePath(new Vector2(points[pointIndex].transform.position.x, points[pointIndex].transform.position.y), pathToPoint);
        if (pathToPoint.status == NavMeshPathStatus.PathComplete)
        {
            agent.isStopped = false;
            agent.SetDestination(points[pointIndex].transform.position);
            anim.SetFloat("movement", 1);
        }
        else
        {
            agent.isStopped = true;
            pointIndex = (pointIndex + 1) % points.Length;
            anim.SetFloat("movement", 0);
        }
        //check if you've gotten to the point
        ReachedPoint();
    }
    private void ReachedPoint()
    {
        if(Vector2.Distance(transform.position, points[pointIndex].transform.position) <= distFromPoint)
                pointIndex = (pointIndex + 1) % points.Length;
    }
    public void TriggerChase()
    {
        seesPlayer = true;
        trigger = false;
        agent.speed = 10;
    }
    void Update()
    {
        bool fov = FOVCheck();
        if (fov)
        {
            //if you don't see player but they're in your field of view start transition
            if(!seesPlayer)
            {
                agent.CalculatePath(target.position, path);
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    trigger = true;
                    agent.SetDestination(new Vector3(target.position.x, target.position.y, target.position.z));
                    agent.isStopped = true;
                    rememberTimer = MAX_REMEMBER_TIMER;
                    anim.Play("Appear");
                }
            }
        }
        Memory();
        if (seesPlayer && !trigger) MoveTowards();
        else if (!seesPlayer && !trigger) Patroling();
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
            if (agent.enabled) agent.SetDestination(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z));
        }
        this.enabled = flag;
        if (agent.enabled) agent.isStopped = !flag;
        //agent.enabled = flag;

    }
    //what happens when disable
    public void EnableByID(int ID)
    {
        if (ID == 0)
            this.enabled = true;
        if (agent.enabled) agent.isStopped = false;
        //agent.enabled = true;
    }
    public void DisableByID(int ID)
    {
        if (ID == 0)
            this.enabled = false;
        if (agent.enabled) agent.isStopped = true;
        //agent.enabled = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireSphere(gameObject.transform.position, radius);
        Gizmos.color = new Color(0, 1, 0, 1f);
        Gizmos.DrawLine(transform.position, transform.position + looking * radius);
        DrawWireArc(gameObject.transform.position, new Vector3(looking.x, 0, looking.y), angle, radius);
    }
    public static void DrawWireArc(Vector3 position, Vector3 dir, float anglesRange, float radius, float maxSteps = 20)
    {
        var srcAngles = GetAnglesFromDir(position, dir);
        var initialPos = position;
        var posA = initialPos;
        var stepAngles = anglesRange / maxSteps;
        var angle = srcAngles - anglesRange / 2;
        for (var i = 0; i <= maxSteps; i++)
        {
            var rad = Mathf.Deg2Rad * angle;
            var posB = initialPos;
            posB += new Vector3(radius * Mathf.Cos(rad), radius * Mathf.Sin(rad), 0);

            Gizmos.DrawLine(posA, posB);

            angle += stepAngles;
            posA = posB;
        }
        Gizmos.DrawLine(posA, initialPos);
    }

    static float GetAnglesFromDir(Vector3 position, Vector3 dir)
    {
        var forwardLimitPos = position + dir;
        var srcAngles = Mathf.Rad2Deg * Mathf.Atan2(forwardLimitPos.z - position.z, forwardLimitPos.x - position.x);

        return srcAngles;
    }
}
