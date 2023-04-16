using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementController : MonoBehaviour, IScriptable
{
    // public adjustable variables
    public float radius;
    [Range(1.0F, 360.0F)] public float angle;
    public bool LockFOVToY;
    public float speed;

    // private internal metrics
    private const float ATTACK_TIMER_MAX = 0.0f;
    private float attackTimer;
    private Vector3 looking;
    private Vector3 lastPos;
    private bool flipLook;

    // level or enemy components
    private EnemyBase enemyController;
    private List<Collider2D> collidersList = new List<Collider2D>();
    Transform target;
    NavMeshAgent agent;
    private NavMeshPath path;

    /*
    ==================== Setup ======================
    */
    void Awake()
    {
        // NavMesh Agent
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        path = new NavMeshPath();

        LookingForDirection();
        lastPos = transform.position;
        target = GeneralFunctions.GetPlayer().transform;

        enemyController = gameObject.GetComponent<EnemyBase>();
    }

    void Start()
    {

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
    public void ScriptHandler(bool flag){
        if(flag){
            attackTimer = ATTACK_TIMER_MAX;
        }
        this.enabled = flag;
    }

    public void Attack()
    {
        GetComponent<AttackManager>().InvokeAttack("SlimeAttack");
    }

    /*
    ============== Enemy Vision ==============
    */
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

    private void GetDirection()
    {
        if (LockFOVToY)
        {
            if (lastPos.y < transform.position.y) flipLook = true;
            if (lastPos.y > transform.position.y) flipLook = false;
        }
        if (lastPos.x < transform.position.x)
        {
            //sr.flipX = true;
            float newX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(newX, transform.localScale.y, transform.localScale.z);
            if(!LockFOVToY) flipLook = false;
        }
        if (lastPos.x > transform.position.x)
        {
            float newX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(-newX, transform.localScale.y, transform.localScale.z);
            if (!LockFOVToY) flipLook = true;
        }
        lastPos = transform.position;
    }

    private void LookingForDirection()
    {
        GetDirection();
        if (!LockFOVToY && !flipLook) looking = Vector3.right;
        else if (!LockFOVToY && flipLook) looking = Vector3.left;
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

    /*
    ========================== Chase =========================
    */

    public void MoveTowards(float minimumDistance)
    {
        if (agent.enabled)
        {
            agent.CalculatePath(target.position, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                agent.isStopped = false;
                agent.SetDestination(new Vector3(target.position.x, target.position.y, transform.position.z));
                enemyController.MoveAnimation();
            }
            else
            {
                agent.isStopped = true;
            }
        }
        
    }

    public bool InRangeOfPlayer(float minimumDistance)
    {
        return Vector2.Distance(transform.position, target.position) <= minimumDistance;
    }

}
