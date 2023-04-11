using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonRanged : MonoBehaviour, IScriptable
{
    public Transform target;
    private GameObject bulletPrefab;
    public float speed;
    public float minimumDistance;
    public float lineOfSightDistance;
    private HealthTracker healthTracker;
    private Animator animator;
    private SpriteRenderer sr;
    private SkeletonEnemyScript ses;
    //gameObject enemy;

    private const float ATTACK_TIMER_MAX = 0.01f;
    private float attackTimer;
    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = Resources.Load("Prefabs/Enemies/Fireball") as GameObject;
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        healthTracker = GetComponent<HealthTracker>();
        target = GameObject.Find("Player").transform;
        ses = GetComponent<SkeletonEnemyScript>();
    }

    public void EnableByID(int ID)
    {
        if (ID == 0)
            this.enabled = true;
    }
    public void DisableByID(int ID)
    {
        if (ID == 0)
            this.enabled = false;
    }
    //enable and disable script
    public void ScriptHandler(bool flag)
    {
        if (flag)
        {
            attackTimer = ATTACK_TIMER_MAX;
        }
        this.enabled = flag;
    }
    public void FinishShoot()
    {
        ses.shooting = false;
    }
    public void Shooting()
    {
        Debug.Log("pew pew");

        GameObject obj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector2 directionToTarget = target.transform.position - transform.position;
        float angle = Vector3.Angle(Vector3.right, directionToTarget);

        if (target.transform.position.y < transform.position.y)
        {
            angle *= -1;
        }
        Quaternion bulletRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //GameObject go = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject; 
        attackTimer = ATTACK_TIMER_MAX;
        //go.transform.parent = GameObject.Find("Slime Monster_0 (4)").transform;
        //GetComponent<AttackManager>().InvokeAttack("SlimeAttack");
        //animator.Play("SlimeAttack");
    }
    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("movement", 0);

        //turn logic
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
            // Too far away from player to see
            transform.position = transform.position;
        }
        else
        {
            // Can see the player
            if (Vector2.Distance(transform.position, target.position) > minimumDistance && ses.shooting == false)
            {
                // Can see player but not in range to attack
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                animator.SetFloat("movement", 1);
            }
            else
            {
                //attack here
                attackTimer -= Time.deltaTime;
                if (attackTimer < 0 && ses.shooting == false)
                {
                    ses.shooting = true;
                    animator.Play("Attack");
                }
            }
        }

    }
}
