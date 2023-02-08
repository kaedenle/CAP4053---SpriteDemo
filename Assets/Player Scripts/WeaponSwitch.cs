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
        equiped = animator.GetBool("equiped");
        //toggle between not equiped and equiped
        if(equip)
            animator.SetBool("equiped", !equiped);
        
        if(!equiped)
            sr.sprite = null;
        else if(equiped){
            if(swap){
                weaponID += 1;
                weaponID %= spriteList.Length;
            }
            sr.sprite = spriteList[weaponID];
        }
        
        
    }
}
