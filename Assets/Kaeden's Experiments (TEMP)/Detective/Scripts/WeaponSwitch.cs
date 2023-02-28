using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour, IScriptable
{

    public Sprite[] spriteList;
    public int weaponID;
    public SpriteRenderer sr;
    public Animator animator;
    public bool equiped;
    AnimatorClipInfo[] m_CurrentClipInfo;
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        weaponID = 0;
        body = gameObject.GetComponent<Rigidbody2D>();
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

    // Update is called once per frame
    void Update()
    {
        //AnimatorClipInfo[] m_CurrentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        //Access the Animation clip name
        //Debug.Log(m_CurrentClipInfo[0].clip.name);

        bool swap = Input.GetButtonDown("Fire1");
        bool equip_press = Input.GetButtonDown("Fire3");
        equiped = animator.GetBool("equiped");

        //toggle between not equiped and equiped
        //Only when you aren't attacking
        if(equip_press && animator.GetFloat("attack") == 0){
            if(!equiped)
                animator.Play("Idle_Engage");
            else
                animator.Play("Idle");
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
            sr.sprite = spriteList[weaponID];
        }

        //input for the player
        if(equiped){
            //make it move cancellable
            if(gameObject.GetComponent<Player_Movement>().move_flag && animator.GetFloat("movement") > 0 && animator.GetFloat("attack") > 0){
                gameObject.GetComponent<AttackManager>().DestroyPlay();
                gameObject.GetComponent<Player_Movement>().move_flag = false;
            }

            //Swing when press left click
            if(Input.GetKeyDown(KeyCode.Mouse0) && animator.GetFloat("attack") == 0){
                //disable scripts 
                //(should be in animator but there's a weird bug with putting functions in an animator event on the first frame)
                //Animator should call ScriptToggle(0) in AttackManager on the first frame of the attack
                gameObject.GetComponent<AttackManager>().ScriptToggle(0);

                //set you're attacking and the ID of the attack
                animator.SetTrigger("Attack");
                animator.SetFloat("attack", 1);

                //damage self by 5 points
                //gameObject.GetComponent<HealthTracker>().healthSystem.Damage(5);
            }
        }
    }

    
}
