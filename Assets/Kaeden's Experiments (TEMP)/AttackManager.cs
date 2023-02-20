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
    public bool active;
    //public Hitbox[] HBList;
    public List<Hitbox> HBList = new List<Hitbox>();
    private int startFrame;
    public GameObject hitboxParent;

    //framedata
    public TextAsset moveContainer;

    [System.Serializable]
    public class FrameData
    {
        //employees is case sensitive and must match the string "employees" in the JSON.
        public Attack[] framedata;
        public int damage;
        public int knockback;
        public int hitstop;
        //move info
        public string hitsTag;      //can make this into an array
        public string[] cancelBy;
        //on hit, deactivate move or hitbox?
        public bool deactivateMove;
        public float x_knockback;
        public float y_knockback;
    }
    public FrameData framedata = new FrameData();

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        HBList.Clear();
        //atk = AttackID.Sword1;
    }

    private void CreateHitbox(Attack a){
        GameObject HitboxPrefab = Resources.Load("Prefabs/Hitboxes") as GameObject;
        GameObject parent = hitboxParent;
        if(hitboxParent == null)
            parent = gameObject;
        GameObject createdHitbox = Instantiate(HitboxPrefab);
        createdHitbox.name = "Hitbox";
        createdHitbox.transform.SetParent(parent.transform);
        
        Hitbox HBObj = createdHitbox.GetComponent<Hitbox>();
        HBObj.Atk = a;
        HBList.Add(HBObj);
    }

    public void DestroyAllHitboxes(){
        int HBListLength = HBList != null ? HBList.Count : 0;
        for(int i = 0; i < HBListLength; i++){
            Hitbox hb = HBList[i];
            Destroy(hb.gameObject);
        }
        HBList.Clear();
    }

    private void UpdateHitboxInfo(Hitbox hb, Attack atk){
        hb.SetAuxillaryValues(framedata.hitstop, framedata.hitsTag, framedata.cancelBy);
        //set default value (when frame's damage/knockback is 0)
        if(atk.damage == 0)
            atk.damage = framedata.damage;
        if(atk.knockback == 0)
            atk.knockback = framedata.knockback;
        if(atk.x_knockback == 0)
            atk.x_knockback = framedata.x_knockback;
        if(atk.y_knockback == 0)
            atk.y_knockback = framedata.y_knockback;
        hb.Atk = atk;
        //set position and scale, then fix rotation
        hb.gameObject.transform.localPosition = new Vector3(hb.Atk.x_pos, hb.Atk.y_pos, 0);
        hb.gameObject.transform.localScale = new Vector3(hb.Atk.x_scale, hb.Atk.y_scale, 0);
        hb.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        hb.Activate(); 
    }

    private void ProvisionHitboxes(){
        int currentFrame = GetAnimationFrame() - startFrame;
        int HBListLength = HBList != null ? HBList.Count : 0;
        int counter = 0;
        foreach(Attack a in framedata.framedata){
            if(a.frame == currentFrame){     
                //create hitboxes if need more
                if(counter >= HBListLength)
                    CreateHitbox(a);
                UpdateHitboxInfo(HBList[counter], a);
                counter += 1;
            }
        }
        //don't destroy existing hitboxes if there's no data on it on the current frame
        if(counter == 0)
            return;

        //update length of HBList
        HBListLength = HBList != null ? HBList.Count : 0;
        //destroy extra hitboxes
        for(int i = counter; i < HBListLength; i++){
            Hitbox hb = HBList[i];
            HBList.RemoveAt(i);
            Destroy(hb.gameObject);
        }
        //HBList = GetComponentsInChildren<Hitbox>();
        //Debug.Log("CURRENT: " + currentFrame);
    }

    private int GetAnimationFrame(){
        AnimatorClipInfo[] m_CurrentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        AnimatorStateInfo animationInfo = animator.GetCurrentAnimatorStateInfo(0);
        return ((int)(animationInfo.normalizedTime % 1 * (m_CurrentClipInfo[0].clip.length * m_CurrentClipInfo[0].clip.frameRate)));
    }

    public void StartPlay(AttackID atk){
        //based on static set
        this.atk = atk;
        //based on holding (need to edit, maybe make implicit to atk object?)
        if(gameObject.GetComponent<WeaponSwitch>().weaponID == 2)
            this.atk = AttackID.Crowbar;

        //get current animation to keep track of current animation frame (attach hitboxes to animation)
        startFrame = GetAnimationFrame();
        Debug.Log("START: " + startFrame);

        //get framedata
        framedata = JsonUtility.FromJson<FrameData>(moveContainer.text);
        //tell hitboxes to update
        active = true;
    }

    public void StopPlay(){
        if(framedata.deactivateMove)
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
            //Read framedata to change hitboxes
            ProvisionHitboxes();
            //object that hit something
            Hitbox current = null;
            foreach (Hitbox box in HBList){
                box.updateHitboxes();
                if(box._state == Hitbox.ColliderState.Colliding){
                    //if new box has smaller ID or current is not set
                    //if(current == null || current.ID > box.ID)
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
