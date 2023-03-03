using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IUnique
{
    // Start is called before the first frame update
    private Animator animator;
    private Rigidbody2D body;

    public void EffectManager(string funct)
    {
        //call function via string reference
        Invoke(funct, 0f);
    }

    private void Sword1()
    {
        bool flipped = animator.GetBool("flipped");
        Vector3 move = new Vector3(30, 0, 0);
        if (flipped) move *= -1;

        body.AddForce(move, ForceMode2D.Impulse);
    }

    public void onDeath()
    {
        Debug.Log("You've died!");
        EntityManager.PlayerDied();
    }

    public void HitStunAni()
    {
        //TEMPORARY
        bool equiped = animator.GetBool("equiped");
        if (equiped)
            animator.Play("Idle_Engaged");
        else
            animator.Play("Idle");
    }

    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        bool flipped = animator.GetBool("flipped");
        int temp = flipped ? 180 : 0;

        //update the Weapon tag to rotate correctly (all will be animated the same)
        GameObject[] wpnObjs = GameObject.FindGameObjectsWithTag("Weapon");
        foreach (GameObject wpn in wpnObjs)
        {
            GameObject parent = wpn.transform.parent.gameObject;
            //flip and flip to default
            if (wpn.transform.rotation.eulerAngles.y < 180 && flipped)
                wpn.transform.RotateAround(parent.transform.position, Vector3.up, 180);
            else if (wpn.transform.rotation.eulerAngles.y >= 180 && !flipped)
                wpn.transform.RotateAround(parent.transform.position, Vector3.up, 0);
        }
    }
}
