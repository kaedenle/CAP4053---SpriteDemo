using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D body;
    public SpriteRenderer sr;
    //public static event Action<Enemy> onEnemyKilled;

    private Vector2 movement;
    public Animator animator;

    Vector2 moveDirection;
    Transform target, thisTarget;
    [SerializeField] float health, maxHealth = 3f;

    [SerializeField] float moveSpeed = 2f;    

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    private void Start(){
        health = maxHealth;
        target = GameObject.Find("Player").transform;
       // thisTarget = GameObject.Find("Enemy").tranform;

    }
    // Update is called once per frame
    void Update()
    {
        //if(target.position == thisTarget)
        //{
        //    moveDirection = new Vector2(0,0);
        //}
        if(target)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //body.rotation = angle;
            moveDirection = direction; 
        }
    }

    private void FixedUpdate()
    {
        if(target)
        {
            body.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        }
    }
}
