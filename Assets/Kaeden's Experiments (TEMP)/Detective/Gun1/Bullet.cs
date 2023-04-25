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
        Destroy(gameObject, 0.5f);
        shootDirection = new Vector3(1, 0, 0);
        //reverse the shot
        Animator animator = ShotFrom?.GetComponent<Animator>();
        if (animator != null && animator.GetBool("flipped") == true)
            shootDirection *= -1;
        transform.localScale = new Vector2(shootDirection.x * transform.localScale.x, transform.localScale.y);
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
