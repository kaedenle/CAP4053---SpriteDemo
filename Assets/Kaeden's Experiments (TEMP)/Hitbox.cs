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
    private LayerMask m_LayerMask;
    public ColliderState _state;

    //attack information
    public AttackManager.AttackID atkID;
    public Attack Atk;

    private int hitstop;
    private string hitsTag;
    private string[] cancelBy;
    

    //colliding information
    public Collider2D[] colliders;

    // Start is called before the first frame update
    void Start()
    {
        m_LayerMask = LayerMask.GetMask("Entity");
        _state = ColliderState.Closed;
        atkID = transform.root.GetComponent<AttackManager>().atk;
        //dummy stats, will need AttackManager to provision Attack object for hitbox
        //need to find a way to make knockback relative (since this is on player get player's position then configure pre-set knockback?)
        //pre-set knockback will assume you're facing right
        //Atk = new Attack(10, 500);
    }

    public void SetAuxillaryValues(int hitstop, string hitsTag, string[] cancelBy){
        this.hitstop = hitstop;
        this.hitsTag = hitsTag;
        this.cancelBy = cancelBy;
    }

    //draw hitbox
    private void OnDrawGizmos() {
        checkGizmoColor();
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.DrawCube(Vector3.zero, transform.localScale);
    }
    public void Activate(){
        _state = ColliderState.Open;
    }
    public void Deactivate(){
        _state = ColliderState.Closed;
        colliders = null;
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

    public void hitSomething(){
        foreach(Collider2D c in colliders){
            //thing you're hitting
            GameObject hitting = c.gameObject;
            //if they don't have an IDamagable who cares
            foreach(IDamagable script in hitting.GetComponents<IDamagable>()){
//-------------------------CHANGE THIS LATER ONCE HAVE FULL ATTACK OBJ-------------------------
                string tempTag = hitsTag == null ? "" : hitsTag;
                if(hitting.tag == tempTag){
                    //set to knockback param of player
                    Vector3 tempKnockBack = new Vector3(Atk.x_knockback, Atk.y_knockback, 0);
                    tempKnockBack = checkKnockback(hitting, tempKnockBack);
                    script.damage(Atk.knockback * tempKnockBack, Atk.damage);
                }    
            }
        }
    }

    private Vector3 checkKnockback(GameObject hitting, Vector3 angle){
        Vector3 comp = hitting.transform.position - gameObject.transform.root.position;
        if(comp.x < 0)
            angle.x *= -1;
        if(comp.y < 0)
            angle.y *= -1;
        return angle;
    }

    public void updateHitboxes() {
        if (_state == ColliderState.Closed) { return; }
        //bool hitFlag = false;
        //atkID = transform.parent.parent.parent.GetComponent<AttackManager>().atk;
        colliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.localScale.x, transform.localScale.y), 0, m_LayerMask);
        _state = colliders.Length > 0 ? ColliderState.Colliding : ColliderState.Open;
    }
    
    
}
