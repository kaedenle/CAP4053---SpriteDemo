using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponManager : MonoBehaviour, IScriptable
{
    public static EventHandler Swapping;
    public Sprite[] spriteList;
    public GameObject WeaponUIInstance;
    public SpriteRenderer sr;
    public int weaponID;
    public int BufferWeaponID = 0;
    private Animator animator;
    public bool equiped;
    public AttackManager.WeaponList wpnList = new AttackManager.WeaponList();
    private Hurtbox hrtbx;
    private SpriteRenderer onhand;
    private bool[] unlocked;
    Player_Movement movementScript;
    private WeaponUI ui;
    private AttackManager am;

    // Start is called before the first frame update

    void Awake()
    {
        weaponID = 0;
        animator = gameObject.GetComponent<Animator>();
        
        hrtbx = gameObject?.GetComponent<Hurtbox>();
        onhand = gameObject.transform.Find("Right Arm").Find("On-Hand").GetComponent<SpriteRenderer>();
        movementScript = gameObject.GetComponent<Player_Movement>();
        WeaponUIInstance = GameObject.Find("/-- UI --/Menu Canvas/WeaponUI");
        unlocked = new bool[wpnList.weaponlist.Length];
        ui = WeaponUIInstance.GetComponent<WeaponUI>();
        am = GetComponent<AttackManager>();
    }
    void Start()
    {
        int counter = 0;
        wpnList = gameObject.GetComponent<AttackManager>().wpnList;
        while (wpnList.weaponlist[wpnList.index].active == false)
        {
            if (counter > wpnList.weaponlist.Length) break;
            wpnList.index++;
            wpnList.index %= wpnList.weaponlist.Length;
            counter++;
        }
    }
    
    //scripts to be disabled/enabled when attacking
    public void ScriptHandler(bool flag)
    {
        this.enabled = flag;
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

    public void SetSprite()
    {
        //set the visual of the weapon from sprite list
        sr.sprite = spriteList[wpnList.index % spriteList.Length];
    }

    // Update is called once per frame
    void Update()
    {
        equiped = animator.GetBool("equiped");

        //toggle between not equiped and equiped
        if(InputManager.EquipKeyDown() && animator.GetFloat("attack") == 0){
            if (equiped)
            {
                onhand.enabled = false;
                //force UI away
                ui.PreemptivelyFinish();
            }   
            else
            {
                onhand.enabled = true;
                PlayerMetricsManager.IncrementKeeperInt("equip");
            }
                
            animator.SetBool("equiped", !equiped);
            //set animation to follow weapon's ID
            animator.SetFloat("weapon", wpnList.weaponlist[wpnList.index].ID);
            
        }
        if (InputManager.SwapKeyDown())
        {
            //equip last when when press swap
            if (!equiped)
            {
                onhand.enabled = true;
                PlayerMetricsManager.IncrementKeeperInt("equip");
                animator.SetBool("equiped", !equiped);
                //set animation to follow weapon's ID
                animator.SetFloat("weapon", wpnList.weaponlist[wpnList.index].ID);
                animator.SetBool("equiped", !equiped);
            }
            //if equiped swap
            if (equiped)
            {
                int original = wpnList.index;
                int count = 0;
                do
                {
                    if (count > spriteList.Length) break;
                    wpnList.index += 1;
                    wpnList.index %= spriteList.Length;
                    count++;
                } while (wpnList.weaponlist[wpnList.index].active == false);
                
                if (WeaponUIInstance != null && !WeaponUIInstance.activeSelf && WeaponUI.render) ui.Invoke();
                else if (WeaponUIInstance != null && WeaponUIInstance.activeSelf && WeaponUI.render) ui.Shift();
                if (wpnList.index != original) PlayerMetricsManager.IncrementKeeperInt("swap");
            }
            
        }
        //change speed if heavy weapon
        if (wpnList.index == 1 && equiped)
            gameObject.GetComponent<Player_Movement>().speed = 8;
        else
            gameObject.GetComponent<Player_Movement>().speed = movementScript.MAX_SPEED;

        //disable weapon from rendering if it's not out  
        if (!equiped)
        {
            sr.sprite = null;
        }
        //input for the player
        if(equiped){
            
            //only swap if not attacking
            if (animator.GetFloat("attack") == 0)
            {
                SetSprite();
                //set animation to follow weapon's ID
                if (animator.GetFloat("weapon") != wpnList.weaponlist[wpnList.index].ID)
                    animator.SetFloat("weapon", wpnList.weaponlist[wpnList.index].ID);
            }
            //make it move cancellable
            if (gameObject.GetComponent<Player_Movement>().move_flag && animator.GetFloat("movement") > 0 && animator.GetFloat("attack") > 0){
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
                    {
                        BufferWeaponID = wpnList.index;
                        am.bufferCancel = wpnList.weaponlist[wpnList.index].attack1;
                    }   
                    else
                        am.InvokeAttack(wpnList.weaponlist[wpnList.index].attack1);

                    //damage self by 5 points
                    //gameObject.GetComponent<HealthTracker>().healthSystem.Damage(5);
                }
                //Swing when press right click
                else if (InputManager.Hit2KeyDown() && wpnList.weaponlist[wpnList.index].attack2 != 0 && animator.GetFloat("shooting") == 0)
                {
                    //call attack from integer (defined by Attack blend tree in animator)
                    if (animator.GetFloat("attack") != 0)
                    {
                        BufferWeaponID = wpnList.index;
                        am.bufferCancel = wpnList.weaponlist[wpnList.index].attack2;
                    }
                    else
                        am.InvokeAttack(wpnList.weaponlist[wpnList.index].attack2);
                }
                if (animator.GetFloat("attack") != 0 && animator.GetBool("Attack") == false)
                {
                    animator.SetTrigger("Attack");
                    PlayerMetricsManager.IncrementKeeperInt("used_" + wpnList.weaponlist[wpnList.index].name);
                }
            }   
        }

        if (animator.GetFloat("attack") == 0 && animator.GetBool("Attack") == true)
            animator.ResetTrigger("Attack");
        
    }

    
}
