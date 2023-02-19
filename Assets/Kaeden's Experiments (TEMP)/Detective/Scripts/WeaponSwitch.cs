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
    private bool move_flag;

    // Start is called before the first frame update
    void Start()
    {
        weaponID = 0;
    }
    void cleanUp(){
        //reactivate scripts
        Debug.Log("Cleaning...");
        //reset trigger for attack animation (can now instantly warp to state whenever again)
        animator.ResetTrigger("Attack");

        //enable all disabled scripts
        scriptHandler(true);

        //deactivate all hitboxes (might have a script to fully destroy all hitbox instances here)
        Hitbox[] HBList = gameObject.GetComponent<AttackManager>().HBList;
        foreach (Hitbox box in HBList)
            box.Deactivate();

        //tell animator you're no longer attacking (for blend tree)
        animator.SetFloat("attack", 0);
        animator.Play("Idle_Engage");
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
            if(move_flag && animator.GetFloat("movement") > 0 && animator.GetFloat("attack") > 0){
                cleanUp();
                move_flag = false;
            }
            //Swing when press left click
            if(Input.GetKeyDown(KeyCode.Mouse0) && animator.GetFloat("attack") == 0){
                //disable scripts
                scriptHandler(false);
                animator.SetTrigger("Attack");
                animator.SetFloat("attack", 1);
                //damage self by 5 points
                //gameObject.GetComponent<HealthTracker>().healthSystem.Damage(5);
            }
        }
    }

    void LateUpdate(){
        bool flipped = animator.GetBool("flipped");
        int temp = flipped ? 180 : 0;
        
        //update the Weapon tag to rotate correctly (all will be animated the same)
        GameObject[] wpnObjs = GameObject.FindGameObjectsWithTag("Weapon");
        foreach(GameObject wpn in wpnObjs) {
            GameObject parent = wpn.transform.parent.gameObject;
            //flip and flip to default
            if(wpn.transform.rotation.eulerAngles.y < 180 && flipped)
                wpn.transform.RotateAround(parent.transform.position, Vector3.up, 180);
            else if(wpn.transform.rotation.eulerAngles.y >= 180 && !flipped)
                wpn.transform.RotateAround(parent.transform.position, Vector3.up, 0);
        }
    }
}
