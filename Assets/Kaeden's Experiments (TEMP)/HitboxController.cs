using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    public enum Attack{
        Sword1,
        Sword2,
        Crowbar
    }
    public Attack atk;
    public Animator animator;
    private bool active;
    private bool move_flag;

    void cleanUp(){
        //reactivate scripts
        Debug.Log("Cleaning...");
        animator.ResetTrigger("Attack");
        scriptHandler(true);
        //deactivate all hitboxes (might have a script to fully destroy all hitbox instances here)
        GetComponentInChildren<Hitbox>().Deactivate();
        //tell animator you're no longer attacking
        animator.SetFloat("attack", 0);
        animator.Play("Idle_Engage");
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
        move_flag = false;
        active = false;
        atk = Attack.Sword1;
    }

    public void StartPlay(Attack atk){
        //based on static set
        this.atk = atk;
        //based on holding
        if(gameObject.GetComponent<WeaponSwitch>().weaponID == 2)
            this.atk = Attack.Crowbar;
        active = true;
        //temporary, turn off hitboxes or add them here
        GetComponentInChildren<Hitbox>().Activate();
        switch(this.atk){
            case(Attack.Sword1):
            {
                Debug.Log("Attacked with Sword1!");
                //get hitbox data here
                break;
            }
            case(Attack.Sword2):
            {
                Debug.Log("Attacked with Sword2!");
                //get hitbox data here
                break;
            }
            case(Attack.Crowbar):
            {
                Debug.Log("Attacked with Crowbar");
                //get hitbox data here
                break;
            }
            default: break;
        }
    }

    public void StopPlay(){
        active = false;
        //temporary, turn off hitboxes or delete hitboxes here
        GetComponentInChildren<Hitbox>().Deactivate();
    }

    // Update is called once per frame
    void Update()
    {
        bool equiped = animator.GetBool("equiped");
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
                gameObject.GetComponent<HealthTracker>().healthSystem.Damage(5);
            }
        }
        //Debug.Log(Sword1.var);
    }
    void LateUpdate(){
        bool flipped = animator.GetBool("flipped");
        int temp = flipped ? 180 : 0;
        
        GameObject[] wpnObjs = GameObject.FindGameObjectsWithTag("Weapon");
        foreach(GameObject wpn in wpnObjs) {
            GameObject parent = wpn.transform.parent.gameObject;
            if(wpn.transform.rotation.eulerAngles.y < 180 && flipped){
                
                wpn.transform.RotateAround(parent.transform.position, Vector3.up, 180);
            }
            else if(wpn.transform.rotation.eulerAngles.y >= 180 && !flipped){
                wpn.transform.RotateAround(parent.transform.position, Vector3.up, 0);
            }
            
            
        }
    }
}
