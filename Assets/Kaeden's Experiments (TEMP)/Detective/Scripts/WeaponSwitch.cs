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
    AnimatorClipInfo[] m_CurrentClipInfo;

    // Start is called before the first frame update
    void Start()
    {
        weaponID = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        //m_CurrentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
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
        
        
    }
}
