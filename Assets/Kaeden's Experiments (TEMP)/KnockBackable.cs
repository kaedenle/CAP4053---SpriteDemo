using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackable : MonoBehaviour, IDamagable
{
    private Rigidbody2D body;
    void Start(){
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    public void damage(Vector3 knockback, int damage){
        if(body == null)
            return;
        //it stores the knockback?
        body.AddForce(knockback, ForceMode2D.Impulse);
        Debug.Log(gameObject.name + " Took Knockback!");
    }
}
