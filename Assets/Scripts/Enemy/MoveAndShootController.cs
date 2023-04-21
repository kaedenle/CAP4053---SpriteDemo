using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndShootController : MovementController
{
    private GameObject bulletPrefab;
    public bool shooting = false;

    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
        bulletPrefab = Resources.Load("Prefabs/Enemies/Fireball") as GameObject;
    }

    void Update()
    {
        Debug.Log("shooting = " + shooting);
    }

    public void FinishShoot()
    {
        shooting = false;
    }

    public override void Attack()
    {
        shooting = true;
        GetComponent<Animator>().Play("Attack");
    }

    public void Shooting()
    {
        Debug.Log("pew pew");

        GameObject obj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector2 directionToTarget = target.transform.position - transform.position;
        float angle = Vector3.Angle(Vector3.right, directionToTarget);

        if (target.transform.position.y < transform.position.y)
        {
            angle *= -1;
        }

        Quaternion bulletRotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
}
