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
    GameObject[] objs;

    public bool flipped;

    void Start(){
        body = GetComponent<Rigidbody2D>(); 
        sr = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");     
        objs = GameObject.FindGameObjectsWithTag("Player");

        if(movement.x != 0){
            flipped = movement.x < 0 ? true : false;
            foreach(GameObject part in objs) {
                part.GetComponent<SpriteRenderer>().flipX=flipped;   
            }
        }

        animator.SetFloat("movement", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        body.MovePosition(body.position + movement * speed * Time.fixedDeltaTime);
    }
}
