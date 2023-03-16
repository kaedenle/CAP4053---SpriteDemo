using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // public bool triggerPause;
    // public bool repeatable;
    public Dialogue dialogue;

    private bool triggered;
    private bool currently_triggered;
    

    // Start is called before the first frame update
    void Start()
    {
        triggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("pressed enter key...");

        if(!currently_triggered && InputManager.ContinueKeyPressed())
        {
            

            // if(repeatable || !triggered) 
            // {
            //     if(triggerPause)
            //     {
            //         EntityManager.Pause();
            //         EntityManager.EnableUIInteractable();
            //     }

            //     triggered = true;
            //     currently_triggered = true;

            // }
        }
    }
}
