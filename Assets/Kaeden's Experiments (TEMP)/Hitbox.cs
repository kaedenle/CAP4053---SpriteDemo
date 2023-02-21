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

    private int hitstop;
    private string hitsTag;
    private string[] cancelBy;
    private bool relativeKnockback;

    //colliding information
    public List<Collider2D> collidersList;

    // Start is called before the first frame update
    void Start()
    {
        collidersList = new List<Collider2D>();
    }

    public void SetAuxillaryValues(int hitstop, string hitsTag, string[] cancelBy, bool relativeKnockback){
        this.hitstop = hitstop;
        this.hitsTag = hitsTag;
        this.cancelBy = cancelBy;
        this.relativeKnockback = relativeKnockback;
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

    public void hitSomething(){
        foreach(Collider2D c in collidersList){
            
            //thing you're hitting
            GameObject hitting = c.gameObject;
            string tempTag = hitsTag == null ? "" : hitsTag;
            string actualTag = hitting.tag;
            if (!hitting.tag.Equals(tempTag))
                continue;
            //if they don't have an IDamagable who cares
//------------------------CAN OPTIMIZE HERE BY HAVING A PRE-DEFINED IDamagable LIST ON HIT OBJECT (SINCE IT'S STATIC)------------------------
            IDamagable[] scripts = hitting?.GetComponent<Hurtbox>()?.damagableScripts;
            if(scripts == null)
                continue;
            foreach(IDamagable script in scripts){
                Vector3 tempKnockBack = (hitting.transform.position - gameObject.transform.root.position).normalized;
                if(!relativeKnockback){
                    //if relativeKnockback is true x_knockback and y_knockback don't matter
                    tempKnockBack = new Vector3(Atk.x_knockback, Atk.y_knockback, 0);
                    tempKnockBack = checkKnockback(hitting, tempKnockBack);
                }
                script.damage(Atk.knockback * tempKnockBack, Atk.damage);
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
        var contactFilter = new ContactFilter2D();
        contactFilter.layerMask = LayerMask.GetMask("Entity");
        contactFilter.useLayerMask = true;
        //contactFilter.NoFilter();
        Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), 
                new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y), 0, contactFilter, collidersList);
        _state = collidersList.Count > 0 ? ColliderState.Colliding : ColliderState.Open;
    }
    
    
}
