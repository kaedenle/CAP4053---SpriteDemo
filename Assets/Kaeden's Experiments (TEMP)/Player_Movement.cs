using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour, IScriptable
{
    public Rigidbody2D body;
    public SpriteRenderer sr;

    public float MAX_SPEED = 12f;
    public float speed;
    private float moveX, moveY;
    private Vector3 movement;
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
        moveX = InputManager.GetAxis("Horizontal");
        moveY = InputManager.GetAxis("Vertical");
        
        if(moveX != 0){
            bool temp = flipped;
            flipped = moveX < 0 ? true : false;
            
            foreach(GameObject part in objs) {
                part.GetComponent<SpriteRenderer>().flipX=flipped;
            }
        }
        animator.SetBool("flipped", flipped);
        movement = new Vector3(moveX, moveY, 0).normalized;
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

    public void EnableByID(int ID){
        cleanUp();
        //move_flag = true;
        if(ID == 0)
            this.enabled = true;
    }

    public void DisableByID(int ID)
    {
        animator.SetFloat("movement", 0);
        //move_flag = false;
        if (ID == 0)
            this.enabled = false;
    }

    void cleanUp(){
        //reset trigger for attack animation (can now instantly warp to state whenever again)
        animator.ResetTrigger("Attack");
        //tell animator you're no longer attacking (for blend tree)
        animator.SetFloat("attack", 0);
        animator.Play(gameObject.GetComponent<WeaponManager>().BlendTree);
    }

    void FixedUpdate()
    {
        gameObject.transform.position += movement * speed * Time.fixedDeltaTime;
        //body.MovePosition(body.position + movement * speed * Time.fixedDeltaTime);
    }
}
