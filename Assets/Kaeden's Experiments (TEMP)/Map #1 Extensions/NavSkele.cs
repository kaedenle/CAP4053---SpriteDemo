using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavSkele : MonoBehaviour, IScriptable, IAI
{

    public float radius;
    [Range(1.0F, 360.0F)]
    public float angle;
    public float speed;
    public Transform target;
    public float minimumDistance;
    public float MAX_REMEMBER_TIMER = 3f;
    public bool LockFOVtoY;
    private HealthTracker healthTracker;
    private Animator animator;
    private SpriteRenderer sr;
    private AttackManager am;
    private Vector3 originalSpot;
    //gameObject enemy;
    NavMeshAgent agent;
    private NavMeshPath path;
    private bool attacking = false;
    private List<Collider2D> collidersList = new List<Collider2D>();
    private Vector3 looking;
    private bool flipLook;
    private Vector3 lastPos;
    private float rememberTimer = 0;
    

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
        LookingForDirection();
        lastPos = transform.position;
    }
    private void GetDirection()
    {
        if (LockFOVtoY)
        {
            if (lastPos.y < transform.position.y) flipLook = true;
            if (lastPos.y > transform.position.y) flipLook = false;
        }
        if (lastPos.x < transform.position.x)
        {
            //sr.flipX = true;
            float newX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(newX, transform.localScale.y, transform.localScale.z);
            if(!LockFOVtoY) flipLook = false;
        }
        if (lastPos.x > transform.position.x)
        {
            float newX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(-newX, transform.localScale.y, transform.localScale.z);
            if (!LockFOVtoY) flipLook = true;
        }
        lastPos = transform.position;
    }
    private void LookingForDirection()
    {
        GetDirection();
        if (!LockFOVtoY && !flipLook) looking = Vector3.right;
        else if (!LockFOVtoY && flipLook) looking = Vector3.left;
        else if (LockFOVtoY && !flipLook) looking = Vector3.down;
        else if (LockFOVtoY && flipLook) looking = Vector3.up;
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
        
        if (collidersList.Count != 01)
        {
            Transform target = collidersList[0].transform;
            //find player
            foreach(Collider2D coll in collidersList)
            {
                if(coll.gameObject.tag == "Player")
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
        if(rememberTimer >= 0) rememberTimer -= Time.deltaTime;
    }
    
    private void MoveTowards()
    {
        animator.SetFloat("movement", 0);
        //Debug.Log(healthTracker.healthSystem.getHealth());
        if (Vector2.Distance(transform.position, target.position) <= 1000f)
        {
            if (Vector2.Distance(transform.position, target.position) > minimumDistance)
            {
                bool fov = FOVCheck();
                if (fov) rememberTimer = MAX_REMEMBER_TIMER;
                if (agent.enabled && (fov || rememberTimer > 0))
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
                if (attackTimer >= 0) attackTimer -= Time.deltaTime;
                if (attackTimer < 0 && !attacking)
                {
                    attacking = true;
                    if (agent.enabled) agent.isStopped = true;
                    am.InvokeAttack("SlimeAttack");
                }
            }
        }
    }
    void Update()
    {
        Memory();
        MoveTowards();
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
