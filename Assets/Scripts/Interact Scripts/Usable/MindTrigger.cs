using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindTrigger : NPCDialogue
{
    [SerializeField] public HubManager.PhaseTag phase;
    private bool triggered;

    // Start is called before the first frame update
    new void Start()
    {
        triggered = false;
        base.Start();
        
        // destroy the object if it's not it's phase
        if (!HubManager.TagIsCurrent(phase))
        {
            Destroy(gameObject);
            return;
        }
    }

    new void Update()
    {
        base.Update();
        triggered |= IsTriggered();

        if (triggered && !UIActive())
        {
            triggered = false;
            HubManager.LoadNextMind();
        }
    }
}
