using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Vector3 shootDirection;
    public float speed;
    private GameObject ShotFrom;
    private AttackManager am;
    //0 means infinite piercing
    public int piercing;

    void Awake()
    {
        am = gameObject?.GetComponent<AttackManager>();
    }
    public void InitBullet(GameObject ShotFrom)
    {
        
        
        this.ShotFrom = ShotFrom;
        //permenatly set hitbox on bullet
        if (am != null) am.StartPlay(0);
        if (am != null) am.ProjectileOwner = ShotFrom;
        Destroy(gameObject, 2);

        shootDirection = new Vector3(1, 0, 0);
        //reverse the shot
        Animator animator = ShotFrom?.GetComponent<Animator>();
        if (animator != null && animator.GetBool("flipped") == true)
            shootDirection *= -1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (shootDirection * speed * Time.deltaTime);
        //single hit
        if (am.NumberHitSomething() >= piercing && piercing > 0)
            Destroy(gameObject);
    }
}
