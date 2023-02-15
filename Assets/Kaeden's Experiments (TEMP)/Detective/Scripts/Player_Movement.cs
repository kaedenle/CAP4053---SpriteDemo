using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public Rigidbody2D body;
    public SpriteRenderer sr;

    public float speed = 5f;
    private float moveX, moveY;
    private bool rotateFlag;
    private Vector2 movement;
    public Animator animator;
    GameObject[] objs;

    public bool flipped;

    void Start(){
        body = GetComponent<Rigidbody2D>(); 
        sr = GetComponent<SpriteRenderer>();
        objs = GameObject.FindGameObjectsWithTag("Player");
        flipped = false;
        rotateFlag = false;
    }
    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        animator.SetBool("flipped", flipped);
        if(moveX != 0){
            bool temp = flipped;
            flipped = moveX < 0 ? true : false;
            //flipped changed
            if(temp != flipped)
                rotateFlag = true;
            
            foreach(GameObject part in objs) {
                part.GetComponent<SpriteRenderer>().flipX=flipped;
            }
        }
        
        movement = new Vector2(moveX, moveY).normalized;
        animator.SetFloat("movement", movement.sqrMagnitude);
            
    }

    void FixedUpdate()
    {
        body.MovePosition(body.position + movement * speed * Time.fixedDeltaTime);
    }
    //late update to bypass animation to flip
    void LateUpdate(){
        //only rotate On-Hand
        int temp = flipped ? 180 : 0;
        GameObject weapon = gameObject.transform.Find("Right Arm").gameObject.transform.Find("On-Hand").gameObject;
        /*weapon.transform.eulerAngles = new Vector3(
            weapon.transform.eulerAngles.x,
            temp,
            weapon.transform.eulerAngles.z
        );*/
        GameObject parent = weapon.transform.parent.gameObject;
        weapon.transform.RotateAround(parent.transform.position, Vector3.up, temp);
        rotateFlag = false;
        

    }
}
