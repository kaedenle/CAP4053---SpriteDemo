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
    public LayerMask m_LayerMask;
    public Vector3 boxSize;
    public ColliderState _state;
    public HitboxController.Attack atk;

    // Start is called before the first frame update
    void Start()
    {
        _state = ColliderState.Closed;
        atk = transform.parent.parent.parent.GetComponent<HitboxController>().atk;
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
    // Update is called once per frame
    private void Update() {
        if (_state == ColliderState.Closed) { return; }
        bool hitFlag = false;
        atk = transform.parent.parent.parent.GetComponent<HitboxController>().atk;
        Collider[] colliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale/2, Quaternion.identity, m_LayerMask);

        for (int i = 0; i < colliders.Length; i++) {
            Collider aCollider = colliders[i];
           // _responder?.collisionedWith(aCollider);
           Debug.Log("Hit " + aCollider.gameObject.name + " with " + atk);
           hitFlag = true;
        }
        if(hitFlag)
            _state = ColliderState.Closed;
        else
            _state = colliders.Length > 0 ? ColliderState.Colliding : ColliderState.Open;

    }
    
    
}
