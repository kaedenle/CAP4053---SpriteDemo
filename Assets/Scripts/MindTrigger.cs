using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindTrigger : MonoBehaviour
{
    [Range(1, 2)] public int startingType = 2;
    [SerializeField] public HubManager.PhaseTag phase;

    private Animator animator;
    private string animatorBool = "playerNear";

    // Start is called before the first frame update
    void Start()
    {
        // destroy the object if it's not it's phase
        if (!HubManager.TagIsCurrent(phase))
        {
            Destroy(gameObject);
            return;
        }

        // set the animator info
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool(animatorBool, false);
    }

    void Update()
    {
        // currently the interact is hardcoded as the "e" key
        // TO DO: make a config file to abstract this
        animator.SetInteger("type", startingType);
        if (Input.GetKeyDown(KeyCode.E) && animator.GetBool(animatorBool))
        {
            HubManager.LoadNextMind();
        }
    }

    public void OnTriggerEnter2D()
    {
        animator.SetBool(animatorBool, true);
    }

    public void OnTriggerExit2D()
    {
        animator.SetBool(animatorBool, false);
    }
}
