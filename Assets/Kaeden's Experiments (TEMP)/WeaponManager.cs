using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponManager : MonoBehaviour, IScriptable
{

    public Sprite[] spriteList;
    public int weaponID;
    public SpriteRenderer sr;
    private Animator animator;
    public bool equiped;
    AnimatorClipInfo[] m_CurrentClipInfo;
    private Rigidbody2D body;
    public AttackManager.WeaponList wpnList = new AttackManager.WeaponList();
    public Hurtbox hrtbx;
    private SpriteRenderer onhand;
    // Start is called before the first frame update
    void Start()
    {
        weaponID = 0;
        body = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        wpnList = gameObject.GetComponent<AttackManager>().wpnList;
        hrtbx = gameObject?.GetComponent<Hurtbox>();
        onhand = gameObject.transform.Find("Right Arm").Find("On-Hand").GetComponent<SpriteRenderer>();
    }
    
    //scripts to be disabled/enabled when attacking
    public void ScriptHandler(bool flag)
    {
        //this.enabled = flag;
    }
    //what happens when disable
    public void EnableByID(int ID)
    {
        if (ID == 1)
            this.enabled = true;
    }
    public void DisableByID(int ID)
    {
        if (ID == 1)
            this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //AnimatorClipInfo[] m_CurrentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        //Access the Animation clip name
        //Debug.Log(m_CurrentClipInfo[0].clip.name);

        //bool swap = Input.GetButtonDown("Fire1");
        //bool equip_press = Input.GetButtonDown("Fire3");
        equiped = animator.GetBool("equiped");

        //toggle between not equiped and equiped
        //Only when you aren't attacking
        if(InputManager.EquipKeyDown() && animator.GetFloat("attack") == 0){
            //disable weapon from rendering if it's not out
            if (!equiped)
                onhand.enabled = true;
            else
                onhand.enabled = false;
            animator.SetBool("equiped", !equiped);
            //set animation to follow weapon's ID
            animator.SetFloat("weapon", wpnList.weaponlist[wpnList.index].ID);
        }


        if(!equiped){
            sr.sprite = null;
        }

        else if(equiped){
            if(InputManager.SwapKeyDown() && animator.GetFloat("attack") == 0){
                wpnList.index += 1;
                wpnList.index %= wpnList.weaponlist.Length;
                //set animation to follow weapon's ID
                animator.SetFloat("weapon", wpnList.weaponlist[wpnList.index].ID);

                //change speed if heavy weapon
                Player_Movement movementScript = gameObject.GetComponent<Player_Movement>();
                if (weaponID == 1)
                    gameObject.GetComponent<Player_Movement>().speed = 10;
                else
                    gameObject.GetComponent<Player_Movement>().speed = movementScript.MAX_SPEED;
            }
            //set the visual of the weapon from sprite list
            sr.sprite = spriteList[wpnList.index % spriteList.Length];

        }

        //input for the player
        if(equiped){
            //make it move cancellable
            if(gameObject.GetComponent<Player_Movement>().move_flag && animator.GetFloat("movement") > 0 && animator.GetFloat("attack") > 0){
                gameObject.GetComponent<AttackManager>().DestroyPlay();
                gameObject.GetComponent<Player_Movement>().move_flag = false;
            }


            bool hitStunVar = hrtbx != null ? hrtbx.inHitStun : false;
            //if in hitstun, don't read input (put pausing mechanics in here)
            if (!hitStunVar)
            {
                //Swing when press left click
                if (InputManager.Hit1KeyDown() && wpnList.weaponlist[wpnList.index].attack1 != 0)
                {
                    //special case for shooting
                    if(animator.GetFloat("shooting") > 0)
                    {
                        PlayerScript ps = GetComponent<PlayerScript>();
                        if (ps.ShootAgain)
                            ps.cancel = true;
                    }
                    //call attack from integer (defined by Attack blend tree in animator)
                    else if (animator.GetFloat("attack") != 0)
                        GetComponent<AttackManager>().bufferCancel = wpnList.weaponlist[wpnList.index].attack1;
                    else
                        GetComponent<AttackManager>().InvokeAttack(wpnList.weaponlist[wpnList.index].attack1);

                    //damage self by 5 points
                    //gameObject.GetComponent<HealthTracker>().healthSystem.Damage(5);
                }
                //Swing when press right click
                else if (InputManager.Hit2KeyDown() && wpnList.weaponlist[wpnList.index].attack2 != 0 && animator.GetFloat("shooting") == 0)
                {
                    //call attack from integer (defined by Attack blend tree in animator)
                    if (animator.GetFloat("attack") != 0)
                        GetComponent<AttackManager>().bufferCancel = wpnList.weaponlist[wpnList.index].attack2;
                    else
                        GetComponent<AttackManager>().InvokeAttack(wpnList.weaponlist[wpnList.index].attack2);
                }
                if (animator.GetFloat("attack") != 0 && animator.GetBool("Attack") == false)
                {
                    animator.SetTrigger("Attack");
                }
            }   
        }

        if (animator.GetFloat("attack") == 0 && animator.GetBool("Attack") == true)
            animator.ResetTrigger("Attack");
        
    }

    
}
