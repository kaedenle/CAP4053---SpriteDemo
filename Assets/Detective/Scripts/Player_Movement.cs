using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public Rigidbody2D body;
    public SpriteRenderer sr;

    public float speed = 5f;
    private float moveX, moveY;
    private Vector2 movement;
    public Animator animator;
    GameObject[] objs;

    public bool flipped;

    void Start(){
        body = GetComponent<Rigidbody2D>(); 
        sr = GetComponent<SpriteRenderer>();
        objs = GameObject.FindGameObjectsWithTag("Player");
        flipped = false;
    }
    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        if(moveX != 0){
            flipped = moveX < 0 ? true : false;
            foreach(GameObject part in objs) {
                part.GetComponent<SpriteRenderer>().flipX=flipped;
            }
        }
        animator.SetBool("flipped", flipped);
        
        movement = new Vector2(moveX, moveY).normalized;
        animator.SetFloat("movement", movement.sqrMagnitude);
            
    }

    void FixedUpdate()
    {
        body.MovePosition(body.position + movement * speed * Time.fixedDeltaTime);
    }
}
