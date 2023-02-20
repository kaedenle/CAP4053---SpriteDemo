using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    //Components
    public Animator animator;
    public GameObject hitboxParent;

    //Component Lists/Arrays
    private IScriptable[] scripts;
    public List<Hitbox> HBList = new List<Hitbox>();

    //technical information
    public bool active;
    private int startFrame;

    //framedata
    public TextAsset moveContainer;
    public enum ScriptTypes{
        Movement
    }

    [System.Serializable]
    public class FrameData
    {
        //numeral data
        public Attack[] framedata;
        public int damage;
        public int knockback;
        public int hitstop;
        public float x_knockback;
        public float y_knockback;

        //move info
        public string hitsTag;      //can make this into an array
        public string[] cancelBy;

        //on hit, deactivate move or hitbox?
        public bool deactivateMove;
        public bool relativeKnockback;
    }
    public FrameData framedata = new FrameData();

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        HBList.Clear();
        scripts = gameObject.GetComponentsInChildren<IScriptable>();
        //atk = AttackID.Sword1;
    }
//-----------------------------HITBOX FUNCTIONS----------------------------------------------------------
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

    private void DestroyAllHitboxes(){
        int HBListLength = HBList != null ? HBList.Count : 0;
        for(int i = 0; i < HBListLength; i++){
            Hitbox hb = HBList[i];
            Destroy(hb.gameObject);
        }
        HBList.Clear();
    }

    private void UpdateHitboxInfo(Hitbox hb, Attack atk){
        hb.SetAuxillaryValues(framedata.hitstop, framedata.hitsTag, framedata.cancelBy, framedata.relativeKnockback);
        //set default value (when frame's damage/knockback is 0)
        atk.damage = atk.damage == 0 ? framedata.damage : atk.damage;
        atk.knockback = atk.knockback == 0 ? framedata.knockback : atk.knockback;
        atk.x_knockback = atk.x_knockback == 0 ? framedata.x_knockback : atk.x_knockback;
        atk.y_knockback = atk.y_knockback == 0 ? framedata.y_knockback : atk.y_knockback;
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
//-----------------------------PLAY ATTACK FUNCTIONS----------------------------------------------------------
    public void StartPlay(int moveIndex){
        //get current animation to keep track of current animation frame (attach hitboxes to animation)
        startFrame = GetAnimationFrame();
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

    public void DestroyPlay(){
        active = false;
        DestroyAllHitboxes();
        ScriptToggle(1);
    }
//-----------------------------SCRIPT INTERFACE FUNCTIONS----------------------------------------------------------
    public void ScriptToggle(int flag){
        bool ret = flag > 0 ? true : false;
        foreach(IScriptable s in scripts){
            s.ScriptHandler(ret);
        }
    }

    public void ScriptActivate(ScriptTypes ID){
        foreach(IScriptable s in scripts){
            s.EnableByID((int)ID);
        }
    }
//-----------------------------HELPER FUNCTIONS----------------------------------------------------------
    private int GetAnimationFrame(){
        AnimatorClipInfo[] m_CurrentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        AnimatorStateInfo animationInfo = animator.GetCurrentAnimatorStateInfo(0);
        return ((int)(animationInfo.normalizedTime % 1 * (m_CurrentClipInfo[0].clip.length * m_CurrentClipInfo[0].clip.frameRate)));
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
