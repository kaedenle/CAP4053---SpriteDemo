using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : MonoBehaviour
{
    public float speed;
    public Transform target;
    public float minimumDistance;
    public float lineOfSightDistance;
    public HealthTracker healthTracker; 
    public Animator animator;
    public SpriteRenderer sr;
    // Update is called once per frame
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        healthTracker = GetComponent<HealthTracker>();
    }
    void Update()
    {


        if(Vector2.Distance(transform.position, target.position) > lineOfSightDistance)
        {
            transform.position = transform.position;
        }
        else
        {
            if(Vector2.Distance(transform.position, target.position) > minimumDistance)
            {
                if(transform.position.x < target.position.x)
                {
                    //sr.flipX = true;
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                    //Debug.Log("Flipping should happen");
                }
                else
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    //sr.flipX = false;
                }
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            else
            {

                animator.Play("SlimeAttack");
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
        if(healthTracker.health <= 0)
        {
            Debug.Log("Dead Ooze boy");
            GameObject.Destroy(gameObject);
            
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
