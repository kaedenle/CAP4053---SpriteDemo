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
    
    private void Start()
    {
        targetPosition = GameObject.FindWithTag("Player").transform;
        Vector2 playerPosition = new Vector2(targetPosition.position.x, targetPosition.position.y);
        //transform.rotation = Quaternion.LookRotation(playerPosition);
        previousPosition = targetPosition.position;
        shootDirection = (targetPosition.position - transform.position).normalized;
        Destroy(gameObject, 5);
        

        // Randomize angle variation between bullets
        float spreadAngle = Random.Range(-20, 20);

        // Take the random angle variation and add it to the initial
        // desiredDirection (which we convert into another angle), which in this case is the players aiming direction
        var x = transform.position.x - targetPosition.transform.position.x;
        var y = transform.position.y - targetPosition.transform.position.y;
        float rotateAngle = spreadAngle + (Mathf.Atan2(y, x) * Mathf.Rad2Deg);
                
// Calculate the new direction we will move in which takes into account 
// the random angle generated
        shootDirection = new Vector2(Mathf.Cos(rotateAngle * Mathf.Deg2Rad), Mathf.Sin(rotateAngle * Mathf.Deg2Rad)).normalized;
        shootDirection *= -1;

        
    }

    private void Update()
    {
        //transform.position = Vector2.MoveTowards(transform.position, previousPosition, speed *  Time.deltaTime);
        transform.position += (shootDirection * speed * Time.deltaTime);

       // transform.Translate((transform.forward * speed * Time.deltaTime));
        //transform.position += transform.forward * speed * Time.deltaTime;


        //transform.Translate(transform.right * speed * Time.deltaTime);


       
    }

    
}
