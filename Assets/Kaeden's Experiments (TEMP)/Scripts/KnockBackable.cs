using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackable : MonoBehaviour, IDamagable
{
    private Rigidbody2D body;
    void Start(){
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    public void damage(AttackData ad){
        if(body == null)
            return;

        body.AddForce(ad.knockback, ForceMode2D.Impulse);
        //body.velocity = new Vector2(knockback.x, knockback.y);
        //Debug.Log(gameObject.name + " Took Knockback! " + ad.knockback);
    }

    void FixedUpdate(){

    }
}
