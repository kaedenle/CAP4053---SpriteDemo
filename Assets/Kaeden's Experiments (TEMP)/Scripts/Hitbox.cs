using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public enum ColliderState {
        Closed,
        Open,
        Colliding
    }

    //box information
    public ColliderState _state;

    //attack information
    public Attack Atk;

    private float hitstop;
    public string hitsTag;
    private bool relativeKnockback;
    private string functCall;
    private GameObject ProjectileOwner;
    public bool marked;
    private int weapon;
    private int attackID;
    private AudioClip audios;

    //colliding information
    public List<Collider2D> collidersList;

    // Start is called before the first frame update
    void Start()
    {
        collidersList = new List<Collider2D>();
    }

    public void SetAuxillaryValues(float hitstop, string hitsTag, bool relativeKnockback, string funct, int weapon, int attackID, AudioClip audio){
        this.hitstop = hitstop;
        this.hitsTag = hitsTag;
        this.relativeKnockback = relativeKnockback;
        this.functCall = funct;
        this.weapon = weapon;
        this.attackID = attackID;
        this.audios = audio;
    }
    public void UpdateHitboxInfo(AttackManager.FrameData framedata, Attack atk, int weapon, AudioClip audio)
    {
        SetAuxillaryValues(framedata.hitstop, framedata.hitsTag, framedata.relativeKnockback, framedata.functCall, weapon, framedata.ID, audio);
        //set default value (when frame's damage/knockback is 0)
        if (atk == null)
            atk = new Attack();
        atk.damage = atk.damage == 0 ? framedata.damage : atk.damage;
        atk.knockback = atk.knockback == 0 ? framedata.knockback : atk.knockback;
        atk.hitstun = atk.hitstun == 0 ? framedata.hitstun : atk.hitstun;
        atk.x_knockback = atk.x_knockback == 0 ? framedata.x_knockback : atk.x_knockback;
        atk.y_knockback = atk.y_knockback == 0 ? framedata.y_knockback : atk.y_knockback;
        Atk = atk;

        //set position and scale, then fix rotation
        gameObject.transform.localPosition = new Vector3(Atk.x_pos, Atk.y_pos, 0);
        gameObject.transform.localScale = new Vector3(Atk.x_scale, Atk.y_scale, 0);
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        Activate();
    }

    //draw hitbox
    private void OnDrawGizmos() {
        checkGizmoColor();
        //Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        //Gizmos.DrawCube(Vector3.zero, transform.localScale);
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
    public void Activate(){
        _state = ColliderState.Open;
    }
    public void Deactivate(){
        _state = ColliderState.Closed;
        collidersList.Clear();
    }

    //change hitbox color depending on state
    private void checkGizmoColor() {
        switch(_state) {
            case ColliderState.Closed:
                Gizmos.color = new Color(1, 0, 0, 0.5f);
                break;
            case ColliderState.Open:
                Gizmos.color = new Color(0, 1, 0, 0.5f);
                break;
            case ColliderState.Colliding:
                Gizmos.color = new Color(0, 0, 1, 0.5f);
                break;
        }
    }

    //find furthest root where tag is == enemy
    private GameObject SearchAttackManger(GameObject part){
        while(part.transform?.parent != null && part.transform.gameObject?.GetComponent<AttackManager>() == null){
            part = part.transform.parent.gameObject;
        }
        return part;
    } 
    public void SetProjectileUser(GameObject user)
    {
        ProjectileOwner = user;
    }
    //return true if you've applied effect
    public bool hitEntity(GameObject hit)
    {
        //get tag
        string tempTag = hitsTag == null ? "" : hitsTag;
        if (!hit.tag.Equals(tempTag))
            return false;
        //get all idamagable scripts
        IDamagable[] scripts = hit?.GetComponent<Hurtbox>()?.damagableScripts;
        if (scripts == null)
            return false;

        //apply damage
        //object with attack manager
        GameObject myGameObject = SearchAttackManger(gameObject);
        if (ProjectileOwner != null) myGameObject = ProjectileOwner;
        Debug.Log(hit.name + " hit by " + weapon + " attack " + attackID);
        foreach (IDamagable script in scripts)
        {
            Vector3 tempKnockBack = (hit.transform.position - myGameObject.transform.position).normalized;
            if (!relativeKnockback)
            {
                //if relativeKnockback is true x_knockback and y_knockback don't matter
                tempKnockBack = new Vector3(Atk.x_knockback, Atk.y_knockback, 0);
                tempKnockBack = checkKnockback(hit, myGameObject, tempKnockBack);
            }
            AttackData ad = new AttackData(Atk, tempKnockBack);
            ad.setAux(hitstop, weapon, attackID, audios);
            script.damage(ad);
        }
        return true;
    }

    private Vector3 checkKnockback(GameObject hitting, GameObject current, Vector3 angle){
        Vector3 comp = hitting.transform.position - current.transform.root.position;
        if(comp.x < 0)
            angle.x *= -1;
        if(comp.y < 0)
            angle.y *= -1;
        return angle.normalized;
    }

    public void updateHitboxes() {
        if (_state == ColliderState.Closed) { return; }
        //bool hitFlag = false; 
        var contactFilter = new ContactFilter2D();

        //can only hit on entity layer (may change)
        contactFilter.layerMask = LayerMask.GetMask("Entity");
        contactFilter.useLayerMask = true;

        Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), 
                new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y), 0, contactFilter, collidersList);
        _state = collidersList.Count > 0 ? ColliderState.Colliding : ColliderState.Open;
        //if there's one element and that element is self, set state to open
        if (collidersList.Count == 1 && collidersList[0].transform.root.gameObject == transform.root.gameObject)
            _state = ColliderState.Open;
    }
    
    
}
