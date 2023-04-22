using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour, IScriptable
{
    public Rigidbody2D body;
    public SpriteRenderer sr;

    [HideInInspector]
    public float MAX_SPEED;
    public float speed;
    private float moveX, moveY;
    private Vector3 movement;
    public Animator animator;
    private float lastNonZeroX = 1;
    private float lastNonZeroY = 0;
    GameObject[] objs;

    public bool move_flag;
    public bool flipped;
    private bool lastFlipped;
    private bool isMoveable = true;

    void Start(){
        body = GetComponent<Rigidbody2D>(); 
        sr = GetComponent<SpriteRenderer>();
        objs = GameObject.FindGameObjectsWithTag("Player");
        flipped = false;
        move_flag = false;
        MAX_SPEED = speed = GameData.GetConfig().GetSpeed(); // overriding speed here so that all entities have relative speeds
        lastFlipped = flipped;
    }
    public Vector3 direction()
    {
        return new Vector3(lastNonZeroX, lastNonZeroY, 0) ; 
    }
    private void SetLastLooked()
    {
        //if player is stationary dont do anything
        if (moveX == 0 && moveY == 0)
            return;

        float tempX = 0, tempY = 0;
        //player only moved in y direction
        if (moveY != 0)
            tempY = moveY;
        //player only moved in x direction
        if (moveX != 0)
            tempX = moveX;
        if (tempX == 0 && tempY == 0)
            return;
        lastNonZeroX = tempX;
        lastNonZeroY = tempY;

    }
    // Update is called once per frame
    void Update()
    {
        moveX = isMoveable ? InputManager.GetAxis("Horizontal") : 0;
        moveY = isMoveable ? InputManager.GetAxis("Vertical") : 0;
        SetLastLooked();
        
        if(moveX != 0) flipped = moveX < 0 ? true : false;
        if (lastFlipped != flipped)
        {
            foreach (GameObject part in objs)
            {
                SpriteRenderer srtemp = part.GetComponent<SpriteRenderer>();
                if (srtemp != null) srtemp.flipX = flipped;
            }
            lastFlipped = flipped;
        }
  
        animator.SetBool("flipped", flipped);
        movement = new Vector3(moveX, moveY, 0).normalized;
        animator.SetFloat("movement", movement.sqrMagnitude);
            
    }
    //what happens when toggle
    public void ScriptHandler(bool flag){
        //stop moving so you can transition to idle in animator
        /*if(!flag && animator.GetFloat("weapon") != 2)
            animator.SetFloat("movement", 0);
        else
            cleanUp();*/
        if(!flag)
            animator.SetFloat("movement", 0);
        if (flag)
            cleanUp();
        isMoveable = flag;
    }

    public void EnableByID(int ID){
        cleanUp();
        //move_flag = true;
        if(ID == 0)
            isMoveable = true;
    }

    public void DisableByID(int ID)
    {
        //if(animator.GetFloat("weapon") != 2) animator.SetFloat("movement", 0);
        animator.SetFloat("movement", 0);
        //move_flag = false;
        if (ID == 0)
            isMoveable = false;
    }

    void cleanUp(){
        //reset trigger for attack animation (can now instantly warp to state whenever again)
        animator.ResetTrigger("Attack");
        //tell animator you're no longer attacking (for blend tree)
        animator.SetFloat("attack", 0);
        animator.Play("Idle_E");
    }

    void FixedUpdate()
    {
        animator.SetBool("Skid", false);
        //gameObject.transform.position += movement * speed * Time.fixedDeltaTime;
        body.AddForce(movement * 200 * speed * Time.fixedDeltaTime);
        float dragCoeffic = 30;
        //kill velocity faster
        if (moveX == 0 && moveY == 0 && body.velocity.sqrMagnitude > 0.1f)
        {
            body.AddForce(-body.velocity * dragCoeffic * speed * Time.fixedDeltaTime);
            animator.SetBool("Skid", true);
        }
        Vector2 xtemp = new Vector2(-body.velocity.x, 0);
        Vector2 ytemp = new Vector2(0, -body.velocity.y);
        float turnCoeffic = 45;
        //reverse velocity faster
        if (moveX > 0 && body.velocity.x < 0 || moveX < 0 && body.velocity.x > 0)
        {
            body.AddForce(xtemp * turnCoeffic * speed * Time.fixedDeltaTime);
        }
            
        if (moveY > 0 && body.velocity.y < 0 || moveY < 0 && body.velocity.y > 0)
        {
            body.AddForce(ytemp * turnCoeffic * speed * Time.fixedDeltaTime);
        }
            

        //body.MovePosition(body.position + movement * speed * Time.fixedDeltaTime);
    }
}
