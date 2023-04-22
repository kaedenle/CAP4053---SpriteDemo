using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
Movement Controller - controls Enemy movement (slime, skeleton ranged, skeleton melee)

Assumptions:
- the medium and large FOVs need to have angles > 180 in order to work
*/
public class MovementController : MonoBehaviour, IScriptable
{
    // public adjustable variables
    [SerializeField] public MovementStats movementConfiguration;
    [SerializeField] public MovementStats.FOVType currentFOVType;
    public bool LockFOVToY;
    public bool defaultLooksLeft;

    // private internal metrics
    public Vector3 looking {get; private set;}
    private bool flipLook;

    // level or enemy components
    protected EnemyBase enemyController;
    protected Hurtbox hurtbox;
    protected List<Collider2D> collidersList = new List<Collider2D>();
    protected Transform target;
    protected NavMeshAgent agent;
    protected NavMeshPath path;

    // tracking variables
    private Vector3 lastSeen;
    private float lastSeenTime;

    // constants
    public const double epsilon_distance = 1e-3;

    /*
    ==================== Setup ======================
    */
    protected void Awake()
    {
        lastSeen = transform.position; // default last seen is current position
        lastSeenTime = Time.time;
        flipLook = transform.localScale.x < 0;  // it's looking in the other direction if it starts backwards
        LookingForDirection();
        target = GeneralFunctions.GetPlayer().transform;
        hurtbox = gameObject.GetComponent<Hurtbox>();

        enemyController = gameObject.GetComponent<EnemyBase>();

        // NavMesh Agent
        agent = GetComponent<NavMeshAgent>();

        if(agent != null)
        {
            agent.speed = movementConfiguration.speed;
            agent.updateRotation = false;
            agent.updateUpAxis = false;

            path = new NavMeshPath();
        }
        else
            Debug.LogWarning("NavMesh not found");
    }

    /* 
    ===================IScriptable Functions==================
    */
    public virtual void EnableByID(int ID)
    {
        if(ID == 0)
            this.enabled = true;
    }

    public virtual void DisableByID(int ID)
    {
        if (ID == 0)
            this.enabled = false;
    }

    //enable and disable script
    public void ScriptHandler(bool flag)
    {
        this.enabled = flag;
    }

    /*
    ====================== Attack =====================
    */
    public virtual void Attack()
    {
        GetComponent<AttackManager>().InvokeAttack("SlimeAttack");
    }

    /*
    ============== Enemy Vision ==============
    */
    private void OnDrawGizmos()
    {
        float radius = movementConfiguration.GetRadius(currentFOVType);
        float angle = movementConfiguration.GetAngle(currentFOVType);

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

    // sets the enemy's physical facing direction (only call if the enemy can see the player)
    private void SetDirection(Vector3 target)
    {
        if (LockFOVToY)
        {
            if (target.y < transform.position.y) flipLook = true;
            if (target.y > transform.position.y) flipLook = false;
        }
        if ((target.x > transform.position.x) ^ defaultLooksLeft)
        {
            //sr.flipX = true;
            float newX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(newX, transform.localScale.y, transform.localScale.z);
            if(!LockFOVToY) flipLook = false;
        }
        if ((target.x < transform.position.x) ^ defaultLooksLeft)
        {
            float newX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(-newX, transform.localScale.y, transform.localScale.z);
            if (!LockFOVToY) flipLook = true;
        }
    }

    public void LookingForDirection()
    {
        // GetDirection();
        if (!LockFOVToY && (!flipLook ^ defaultLooksLeft)) looking = Vector3.right;
        else if (!LockFOVToY && (flipLook ^ defaultLooksLeft)) looking = Vector3.left;
        else if (LockFOVToY && !flipLook) looking = Vector3.down;
        else if (LockFOVToY && flipLook) looking = Vector3.up;
    }

    public bool FOVCheck()
    {
        LookingForDirection();
        var contactFilter = new ContactFilter2D();
        bool ret = false;
        //can only hit on entity layer (may change)
        contactFilter.layerMask = LayerMask.GetMask("Entity") | LayerMask.GetMask("Action");
        contactFilter.useLayerMask = true;

        collidersList.Clear();
        Physics2D.OverlapCircle(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y),
                movementConfiguration.GetRadius(currentFOVType), contactFilter, collidersList);
        
        if (collidersList.Count != 0)
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
            float angle = movementConfiguration.GetAngle(currentFOVType);

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

        if(ret)
        {
            lastSeen = target.transform.position;
            lastSeenTime = Time.time;
            SetDirection(lastSeen);
        }
        return ret;
    }

    public void UpdateVision(MovementStats.FOVType visionType)
    {
        currentFOVType = visionType;
    }

    public void TurnAround()
    {
        flipLook = !flipLook;
        transform.localScale = new Vector3(- transform.localScale.x, transform.localScale.y, transform.localScale.z);
        Debug.Log("TurnAround()");
    }

    /*
    ========================== Chase =========================
    */
    public void MoveTowards(Vector2 target)
    {
        if(hurtbox.inHitStun)
        {
            agent.isStopped = true;
        }

        else
        {
            agent.CalculatePath(target, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                agent.isStopped = false;
                agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
            }
            else
            {
                agent.isStopped = true;
            }
        }
        
    }

    public virtual void StopMoving()
    {
        if(agent != null) agent.isStopped = true;
    }

    // chase the last seen position of the player
    public virtual void Chase()
    {
        MoveTowards(lastSeen);
        // transform.position = Vector2.MoveTowards(transform.position, lastSeen, movementConfiguration.speed * Time.deltaTime);
    }

    public bool InRangeOfPlayer()
    {
        return Vector2.Distance(transform.position, target.position) <= movementConfiguration.minimumDistance;
    }

    public bool AtLastSeen()
    {
        return Vector2.Distance(transform.position, lastSeen) <= epsilon_distance;
    }

    public float TimeSinceLastSeen()
    {
        return Time.time - lastSeenTime;
    }

}
