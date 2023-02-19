using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public enum AttackID{
        Sword1,
        Sword2,
        Crowbar
    }

    public AttackID atk;
    public Animator animator;
    private bool active;
    public Hitbox[] HBList;

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        //atk = AttackID.Sword1;
    }

    public void StartPlay(AttackID atk){
        //based on static set
        this.atk = atk;
        //based on holding (need to edit, maybe make implicit to atk object?)
        if(gameObject.GetComponent<WeaponSwitch>().weaponID == 2)
            this.atk = AttackID.Crowbar;

        active = true;
        //temporary, turn off hitboxes or add them here
        HBList = GetComponentsInChildren<Hitbox>();
        foreach (Hitbox box in HBList){
            box.Activate();
            //Debug.Log(box.gameObject.transform.parent.name);
        }
    }

    public void StopPlay(){
        active = false;
        //temporary, turn off hitboxes or delete hitboxes here
        foreach (Hitbox box in HBList)
            box.Deactivate();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Sword1.var);
        //update each hitbox
        if(active){
            //object that hit something
            Hitbox current = null;
            foreach (Hitbox box in HBList){
                box.updateHitboxes();
                if(box._state == Hitbox.ColliderState.Colliding){
                    //if new box has smaller ID or current is not set
                    if(current == null || current.ID > box.ID)
                        current = box;
                }
            }
            //if something hit deactivate all hitboxes
            if(current != null){
                current.hitSomething();
                StopPlay();
                //Debug.Log("Hitbox " + current.ID + " hit");
            }
        }
        
    }

    
}
