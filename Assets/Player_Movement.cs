using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public Rigidbody2D body;
    public SpriteRenderer sr;

    public float speed = 5f;
    private Vector2 movement;
    public Animator animator;

    public bool up;
    public bool down;
    public bool side;

    void Start(){
        body = GetComponent<Rigidbody2D>(); 
        sr = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");     

        if(movement.x > 0){
            sr.flipX = false;
        }
        else if(movement.x < 0){
            sr.flipX = true;
        }

        animator.SetFloat("movement", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        body.MovePosition(body.position + movement * speed * Time.fixedDeltaTime);
    }
}
