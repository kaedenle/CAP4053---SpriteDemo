using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : MonoBehaviour, IScriptable, IAI
{
    public float speed;
    public Transform target;
    public float minimumDistance;
    public float lineOfSightDistance;
    private HealthTracker healthTracker; 
    private Animator animator;
    private SpriteRenderer sr;
    //gameObject enemy;

    private const float ATTACK_TIMER_MAX = 0.0f;
    private float attackTimer;

    // Update is called once per frame
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        healthTracker = GetComponent<HealthTracker>();
        target = GameObject.Find("Player").transform;

        // assumption is speed is based on the player's being 12
        speed = (speed / 12f) * GameData.GetConfig().GetSpeed();
    }

    //enable and disable script
    public void ScriptHandler(bool flag){
        if(flag){
            attackTimer = ATTACK_TIMER_MAX;
        }
        this.enabled = flag;
    }

    void destroyEnemy()
    {
        //Destroy(enemy);
    }
    //what happens when disable
    public void EnableByID(int ID){
        if(ID == 0)
            this.enabled = true;
    }
    public void DisableByID(int ID)
    {
        if (ID == 0)
            this.enabled = false;
    }

    void Update()
    {

        //Debug.Log(healthTracker.healthSystem.getHealth());
        if(transform.position.x < target.position.x)
        {
            //sr.flipX = true;
            float newX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(-newX, transform.localScale.y, transform.localScale.z);
            //Debug.Log("Flipping should happen");
        }
        else
        {
            float newX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(newX, transform.localScale.y, transform.localScale.z);
            //sr.flipX = false;
        }
        if(Vector2.Distance(transform.position, target.position) > lineOfSightDistance)
        {
            transform.position = transform.position;
        }
        else
        {
            if(Vector2.Distance(transform.position, target.position) > minimumDistance)
            {
                
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            else
            {   
                //attack here
                attackTimer -= Time.deltaTime;
                if(attackTimer < 0){
                    GetComponent<AttackManager>().InvokeAttack("SlimeAttack");
                }
            }
        }
/*
        if(Vector2.Distance(transform.position, target.position) > minimumDistance)
        {
            if(transform.position.x < target.position.x)
            {
                sr.flipX = true;
                //Debug.Log("Flipping should happen");
            }
            else
            {
                sr.flipX = false;
            }
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else
        {
            animator.Play("SlimeAttack");
            
            
        }
*/
        /*if(healthTracker.healthSystem.getHealth() <= 0)
        {
            Debug.Log("Dead Ooze boy");
            GameObject.Destroy(healthTracker.bar.gameObject);
            GameObject.Destroy(gameObject);
            
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
     {
         if (collision.transform.name == "Player")//or tag
         {
             //collision.GetComponent<HealthTracker>().damage(new Vector2(10,10), 5);
         }
     }
}
