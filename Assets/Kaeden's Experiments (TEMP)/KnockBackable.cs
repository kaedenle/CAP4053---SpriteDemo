using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackable : MonoBehaviour, IDamagable
{
    private Rigidbody2D body;
    void Start(){
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    public void damage(Vector3 knockback, int damage, float hitstun){
        if(body == null)
            return;

        body.AddForce(knockback, ForceMode2D.Impulse);
        //body.velocity = new Vector2(knockback.x, knockback.y);
        Debug.Log(gameObject.name + " Took Knockback!");
    }

    void FixedUpdate(){

    }
}
