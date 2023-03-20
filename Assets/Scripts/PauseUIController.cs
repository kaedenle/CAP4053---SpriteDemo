using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUIController : MonoBehaviour
{
    private Animator animator;
    private string trigger = "pause";
    private bool triggered = false;
    private bool delayComplete = false;
    private float delay = 1.2F;

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
            StartCoroutine(StartDelay());
        }

        else if(triggered && delayComplete && !UIManager.IsPaused())
        {
            delayComplete = false;
            animator.SetBool(trigger, false);
            triggered = false;
        }
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSecondsRealtime(delay);
        delayComplete = true;
    }
}
