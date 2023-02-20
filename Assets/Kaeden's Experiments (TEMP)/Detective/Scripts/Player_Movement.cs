using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour, IScriptable
{
    public Rigidbody2D body;
    public SpriteRenderer sr;

    public float speed = 5f;
    private float moveX, moveY;
    private Vector2 movement;
    public Animator animator;
    GameObject[] objs;

    public bool move_flag;
    public bool flipped;

    void Start(){
        body = GetComponent<Rigidbody2D>(); 
        sr = GetComponent<SpriteRenderer>();
        objs = GameObject.FindGameObjectsWithTag("Player");
        flipped = false;
        move_flag = false;
    }
    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        
        if(moveX != 0){
            bool temp = flipped;
            flipped = moveX < 0 ? true : false;
            
            foreach(GameObject part in objs) {
                part.GetComponent<SpriteRenderer>().flipX=flipped;
            }
        }
        animator.SetBool("flipped", flipped);
        movement = new Vector2(moveX, moveY).normalized;
        animator.SetFloat("movement", movement.sqrMagnitude);
            
    }
    //what happens when toggle
    public void ScriptHandler(bool flag){
        if(!flag)
            animator.SetFloat("movement", 0);
        else
            cleanUp();
        this.enabled = flag;
    }

    //what happens when disable
    public void EnableByID(int ID){
        move_flag = true;
        if(ID == 0)
            this.enabled = true;
    }

    void cleanUp(){
        //reset trigger for attack animation (can now instantly warp to state whenever again)
        animator.ResetTrigger("Attack");
        //tell animator you're no longer attacking (for blend tree)
        animator.SetFloat("attack", 0);
        animator.Play("Idle_Engage");
    }

    void FixedUpdate()
    {
        body.MovePosition(body.position + movement * speed * Time.fixedDeltaTime);
    }
}
