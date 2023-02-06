using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public Sprite[] spriteList;
    public int weaponID;
    public SpriteRenderer sr;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        weaponID = 0;
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        bool swap = Input.GetButtonDown("Fire1");
        bool equip = Input.GetButtonDown("Fire3");
        //toggle between not equiped and equiped
        if(equip)
            animator.SetBool("equiped", !animator.GetBool("equiped"));
        
        if(animator.GetBool("equiped") && swap){
            weaponID += 1;
            weaponID %= spriteList.Length;
            sr.sprite = spriteList[weaponID];
        } 
    }
}
