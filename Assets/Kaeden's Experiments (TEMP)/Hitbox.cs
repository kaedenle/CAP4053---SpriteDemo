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
    public Vector3 boxSize;
    public ColliderState _state;
    public int ID;

    //attack information
    public AttackManager.AttackID atkID;
    private Attack Atk;

    //colliding information
    public Collider2D[] colliders;

    // Start is called before the first frame update
    void Start()
    {
        m_LayerMask = LayerMask.GetMask("Entity");
        _state = ColliderState.Closed;
        atkID = transform.parent.parent.parent.GetComponent<AttackManager>().atk;
        //dummy stats, will need AttackManager to provision Attack object for hitbox
        //need to find a way to make knockback relative (since this is on player get player's position then configure pre-set knockback?)
        //pre-set knockback will assume you're facing right
        Atk = new Attack(10, 500);
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
                if(hitting.tag == "Enemy")
                    script.damage(Atk.knockback * (hitting.transform.position - gameObject.transform.root.position), Atk.damage);
            }
        }
    }

    public void updateHitboxes() {
        if (_state == ColliderState.Closed) { return; }
        //bool hitFlag = false;
        //atkID = transform.parent.parent.parent.GetComponent<AttackManager>().atk;
        colliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.localScale.x, transform.localScale.y), 0, m_LayerMask);
        _state = colliders.Length > 0 ? ColliderState.Colliding : ColliderState.Open;
    }
    
    
}
