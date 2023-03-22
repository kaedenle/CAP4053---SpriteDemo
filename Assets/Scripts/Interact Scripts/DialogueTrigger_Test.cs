using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger_Test : MonoBehaviour
{
    // public bool triggerPause;
    // public bool repeatable;
    public bool loop_last;
    public Dialogue[] interactives;

    private int interactive_index;
    private InteractiveUIController UI;
    
    

    // Start is called before the first frame update
    void Start()
    {
        interactive_index = 0;
        UI = FindObjectOfType<InteractiveUIController>();
    }

    void TriggerInteractive(Dialogue current)
    {
        UI.StartDialogue(current);
    }

    // Update is called once per frame
    void Update()
    {
        if(!UI.IsActive() && InputManager.ContinueKeyPressed())
        {
            if(interactive_index <= interactives.Length)
            {
                TriggerInteractive(interactives[interactive_index]);
                if(!loop_last || interactive_index + 1 < interactives.Length) interactive_index++;
            }
        }
    }
}
