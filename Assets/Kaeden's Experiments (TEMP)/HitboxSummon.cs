using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxSummon : MonoBehaviour
{
    public enum ColliderState {
        Closed,
        Open,
        Colliding
    }
    public LayerMask m_LayerMask;
    public Vector3 boxSize;
    public ColliderState _state;

    // Start is called before the first frame update
    void Start()
    {
        _state = ColliderState.Open;
    }
    //draw hitbox
    private void OnDrawGizmos() {
        checkGizmoColor();
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

        Gizmos.DrawCube(Vector3.zero, transform.localScale);

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
        Collider[] colliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale/2, Quaternion.identity, m_LayerMask);
        if (colliders.Length > 0) {
            _state =  ColliderState.Colliding;
            // We should do something with the colliders
        } else {
            _state =  ColliderState.Open;
        }
    }
    
    
}
