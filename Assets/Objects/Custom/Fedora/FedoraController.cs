using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FedoraController : MonoBehaviour
{
    [Range(1, 3)] public int startingType = 2;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("playerNear", false);
        animator.SetInteger("id", startingType);
    }

    public void OnTriggerEnter2D()
    {
        animator.SetBool("playerNear", true);
    }

    public void OnTriggerExit2D()
    {
        animator.SetBool("playerNear", false);
    }
}
