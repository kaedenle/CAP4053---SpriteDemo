using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackable : MonoBehaviour, IDamagable
{
    private Rigidbody2D body;
    void Start(){
        body = gameObject.GetComponent<Rigidbody2D>();
    }
    
    public void damage(Vector2 knockback, int damage){
        body.AddForce(knockback);
        Debug.Log("Took Knockback!");
    }
}
