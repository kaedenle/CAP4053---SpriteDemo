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

        side = animator.GetBool("Side");
        up = animator.GetBool("Up");
        down = animator.GetBool("Down");
        
        if(movement.x == 0){
            if(movement.y > 0){
                animator.SetBool("Up", true);
                animator.SetBool("Side", false);
                animator.SetBool("Down", false);
            }
            else if(movement.y < 0){
                animator.SetBool("Up", false);
                animator.SetBool("Side", false);
                animator.SetBool("Down", true);
            }
        }
        
        if(movement.y == 0){
            if(movement.x != 0 ){
                animator.SetBool("Side", true);
                animator.SetBool("Up", false);
                animator.SetBool("Down", false);
            }

            if(movement.x > 0){
                sr.flipX = true;
            }
            else if(movement.x < 0){
                sr.flipX = false;
            }
        }

        if(movement.y != 0 || movement.x != 0){
            animator.SetBool("Moving", true);
        }
        else{
            animator.SetBool("Moving", false); 
        }

        animator.SetFloat("Horizontal", Mathf.Abs(movement.x));
        animator.SetFloat("Vertical", movement.y);
    }

    void FixedUpdate()
    {
        body.MovePosition(body.position + movement * speed * Time.fixedDeltaTime);
    }
}
