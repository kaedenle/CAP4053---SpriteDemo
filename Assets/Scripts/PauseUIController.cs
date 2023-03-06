using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUIController : MonoBehaviour
{
    private Animator animator;
    private string trigger = "pause";
    private bool triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if(!triggered && UIManager.IsPaused())
        {
            animator.SetBool(trigger, true);
            triggered = true;
        }

        else if(triggered && !UIManager.IsPaused())
        {
            animator.SetBool(trigger, false);
            triggered = false;
        }
    }
}
