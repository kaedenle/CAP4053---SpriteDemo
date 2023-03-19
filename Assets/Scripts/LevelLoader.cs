using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Assumption: There is only 1 animator child component attached to this game object
public class LevelLoader : MonoBehaviour
{
    private Animator animator;
    private string trigger = "play";
    // private float transitionTime = 1f;
    private bool triggered = false;

    void Start()
    {   
        animator = gameObject.GetComponentsInChildren<Animator>()[0];
    }

    void Update()
    {
        if(!triggered && UIManager.SceneSwitching())
        {
            triggered = true;
            Fade();
        }
    }

    public void Fade()
    {
        animator.SetTrigger(trigger);
    }
}
