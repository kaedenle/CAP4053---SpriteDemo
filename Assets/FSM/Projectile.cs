using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform targetPosition;
    //public float speedster;
    public float speed;
    private Vector2 previousPosition;
    private Vector3 shootDirection;
    private AttackManager am;
    
    private void Start()
    {
        targetPosition = GameObject.FindWithTag("Player").transform;
        am = gameObject?.GetComponent<AttackManager>();

        // normal shoot at player the player
        Vector2 playerPosition = new Vector2(targetPosition.position.x, targetPosition.position.y);
        //transform.rotation = Quaternion.LookRotation(playerPosition);
        previousPosition = targetPosition.position;
        shootDirection = (targetPosition.position - transform.position).normalized;
        //permenatly set hitbox on bullet
        if (am != null) am.StartPlay(0);
        Destroy(gameObject, 5);
        

        // Randomize angle variation between bullets
        float spreadAngle = Random.Range(-20, 20);

       
       // spread shots at the player
        var x = transform.position.x - targetPosition.transform.position.x;
        var y = transform.position.y - targetPosition.transform.position.y;
        float rotateAngle = spreadAngle + (Mathf.Atan2(y, x) * Mathf.Rad2Deg);
                

        shootDirection = new Vector2(Mathf.Cos(rotateAngle * Mathf.Deg2Rad), Mathf.Sin(rotateAngle * Mathf.Deg2Rad)).normalized;
        shootDirection *= -1;

        
    }

    private void Update()
    {
        //transform.position = Vector2.MoveTowards(transform.position, previousPosition, speed *  Time.deltaTime);
        transform.position += (shootDirection * speed * Time.deltaTime);
        if(transform.position.x > 20 || transform.position.y > 20)
        {
            Destroy(gameObject);
        }
       // transform.Translate((transform.forward * speed * Time.deltaTime));
        //transform.position += transform.forward * speed * Time.deltaTime;


        //transform.Translate(transform.right * speed * Time.deltaTime);


       
    }

    
}
