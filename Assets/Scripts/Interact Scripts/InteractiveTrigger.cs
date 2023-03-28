using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveTrigger : MonoBehaviour
{
    // public bool triggerPause;
    // public bool repeatable;
    public InteractiveText interactives;

    private int interactive_index;
    private InteractiveUIController UI;
    
    

    // Start is called before the first frame update
    void Start()
    {
        interactive_index = 0;
        UI = FindObjectOfType<InteractiveUIController>();
    }

    void TriggerInteractive(string[] current)
    {
        UI.StartInteractive(current, false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!UI.IsActive() && InputManager.ContinueKeyPressed())
        {
            if(interactive_index <= interactives.Length())
            {
                TriggerInteractive(interactives.GetUnit(interactive_index));
                interactive_index = interactives.CalcNextIndex(interactive_index);
               
            }
        }
    }
}
