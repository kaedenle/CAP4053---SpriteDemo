using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour, IScriptable
{
    public float speed;
    public Transform target;
    public float minimumDistance;
    public float lineOfSightDistance;
    public HealthTracker healthTracker; 
    public Animator animator;
    public SpriteRenderer sr;
    public GameObject bulletPrefab;
    //gameObject enemy;

    private const float ATTACK_TIMER_MAX = 1.5f;
    private float attackTimer;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        healthTracker = GetComponent<HealthTracker>();
    }

    public void EnableByID(int ID){
        if(ID == 0)
            this.enabled = true;
    }
    public void DisableByID(int ID)
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
    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, target.position) > lineOfSightDistance)
        {
            // Too far away from player to see
            transform.position = transform.position;
        }
        else
        {
            // Can see the player
            if(Vector2.Distance(transform.position, target.position) > minimumDistance)
            {
                // Can see player but not in range to attack
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            else
            {   
                //attack here
                attackTimer -= Time.deltaTime;
                if(attackTimer < 0){
                    //Debug.Log("pew pew");
   
                    Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                    Vector2 directionToTarget = target.transform.position - transform.position;
                    float angle = Vector3.Angle(Vector3.right, directionToTarget);

                    if(target.transform.position.y < transform.position.y)
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
            }
        }
        
    }
}
