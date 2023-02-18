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
    public LayerMask m_LayerMask;
    public Vector3 boxSize;
    public ColliderState _state;
    public int ID;

    //attack information
    public AttackManager.Attack atkID;
    private IDamagable _responder = null;
    private Attack Atk;

    //colliding information
    public Collider[] colliders;

    // Start is called before the first frame update
    void Start()
    {
        _state = ColliderState.Closed;
        atkID = transform.parent.parent.parent.GetComponent<AttackManager>().atk;
        //dummy stats, will need AttackManager to provision Attack object for hitbox
        Atk = new Attack(10, 5, new Vector2(0, 1));
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
        foreach(Collider c in colliders){
            //thing you're hitting
            GameObject hitting = c.gameObject;
            if (hitting.GetComponent<IDamagable>() != null)
                hitting.GetComponent<IDamagable>().damage(Atk.knockBack.magnitude, Atk.damage);
            
        }
    }

    public void updateHitboxes() {
        if (_state == ColliderState.Closed) { return; }
        //bool hitFlag = false;
        //atkID = transform.parent.parent.parent.GetComponent<AttackManager>().atk;
        colliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale/2, Quaternion.identity, m_LayerMask);
        _state = colliders.Length > 0 ? ColliderState.Colliding : ColliderState.Open;
    }
    
    
}
