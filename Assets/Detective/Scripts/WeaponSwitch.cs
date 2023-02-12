using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public Sprite[] spriteList;
    public int weaponID;
    public SpriteRenderer sr;
    public Animator animator;
    public bool equiped;
    private bool move_flag;

    //for transitioning to idle and idle engage
    void idle(){
        animator.Play("Idle");
    }
    void idleEngage(){
        scriptHandler(true);
        animator.Play("Idle_Engage");
        animator.SetFloat("attack", 0);
        move_flag = false;
    }

    //scripts to be disabled/enabled when attacking
    void scriptHandler(bool flag){
        gameObject.GetComponent<Player_Movement>().enabled = flag;
        //stop player movement
        animator.SetFloat("movement", 0);
    }
    
    void moveToggle(){
        move_flag = true;
        gameObject.GetComponent<Player_Movement>().enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        weaponID = 0;
        move_flag = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool swap = Input.GetButtonDown("Fire1");
        bool equip_press = Input.GetButtonDown("Fire3");
        equiped = animator.GetBool("equiped");
        
        //toggle between not equiped and equiped
        //Only when you aren't attacking
        if(equip_press && animator.GetFloat("attack") == 0){
            if(!equiped)
                idleEngage();
            else
                idle();

            animator.SetBool("equiped", !equiped);
        }

        if(!equiped){
            sr.sprite = null;
        }
        else if(equiped){
            if(swap){
                weaponID += 1;
                weaponID %= spriteList.Length;
            } 
            if(move_flag && animator.GetFloat("movement") > 0 && animator.GetFloat("attack") > 0){
                idleEngage();
                move_flag = false;
            }
            sr.sprite = spriteList[weaponID];
            //Swing when press left click
            if(Input.GetKeyDown(KeyCode.Mouse0) && animator.GetFloat("attack") == 0){
                //disable scripts
                //animator.Play("Sword_One");
                scriptHandler(false);
                animator.SetFloat("attack", 1);
            }
            
                
        }
        
        
    }
}
