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
        body.AddForce(knockback);
        Debug.Log(gameObject.name + " Took Knockback!");
    }
}
