using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathUIController : MonoBehaviour
{
    private Animator animator;
    private string trigger = "trigger";
    private bool triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if(!triggered && LevelManager.PlayerDead())
        {
            animator.SetTrigger(trigger);
            triggered = true;
        }
    }

    public void ResetButton()
    {
        LevelManager.RestartLevel();
    }
}
