using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FedoraController : MonoBehaviour
{
    [Range(1, 3)] public int startingType = 3;
    [SerializeField] public HubManager.PhaseTag phase;

    private Animator animator;
    private string animatorBool = "playerNear";

    // Start is called before the first frame update
    void Start()
    {
        // destroy the object if it's not it's phase
        if(!HubManager.TagIsCurrent(phase))
        {
            Destroy(gameObject);
            return;
        }

        // set the animator info
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool(animatorBool, false);
        animator.SetInteger("id", startingType);
    }

    void Update()
    {
        // currently the interact is hardcoded as the "e" key
        // TO DO: make a config file to abstract this
        if(Input.GetKeyDown(KeyCode.E) && animator.GetBool(animatorBool))
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
