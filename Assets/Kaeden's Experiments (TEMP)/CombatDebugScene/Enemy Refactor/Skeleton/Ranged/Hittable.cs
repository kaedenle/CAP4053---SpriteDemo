using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour, IDamagable
{
    private Projectile proj;
    private Animator anim;
    // Start is called before the first frame update
    public void damage(AttackData ad)
    {
        //destory game object if hit
        gameObject.GetComponent<Projectile>().enabled = false;
        anim.Play("Wilt");
    }
    public void KillProj()
    {
        Destroy(gameObject);
    }
    void Start()
    {
        proj = GetComponent<Projectile>();
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
