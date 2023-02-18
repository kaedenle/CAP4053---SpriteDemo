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

    void FixedUpdate()
    {
        body.MovePosition(body.position + movement * speed * Time.fixedDeltaTime);
    }
    //late update to bypass animation to flip
    void LateUpdate(){
        //GameObject wpn = gameObject.transform.Find("Right Arm").transform.Find("On-Hand").gameObject;
        //AnimatorClipInfo[] m_CurrentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        //int currentFrame = (int) (m_CurrentClipInfo[0].weight * (m_CurrentClipInfo [0].clip.length * m_CurrentClipInfo[0].clip.frameRate));
        //Debug.Log(flipped + " " + wpn.transform.rotation.eulerAngles.y + " " + wpn.name + " frame " + currentFrame + " " + m_CurrentClipInfo[0].clip.name);
        //GameObject weapon = gameObject.transform.Find("Right Arm").gameObject.transform.Find("On-Hand").gameObject;
        /*weapon.transform.eulerAngles = new Vector3(
            weapon.transform.eulerAngles.x,
            temp,
            weapon.transform.eulerAngles.z
        );*/
        
    }
}
