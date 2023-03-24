using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindTrigger : NPCDialogue
{
    [SerializeField] public HubManager.PhaseTag phase;

    // Start is called before the first frame update
    new void Start()
    {
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

        if (base.IsPlayerNear() && InputManager.InteractKeyDown())
        {
            HubManager.LoadNextMind();
        }
    }
}
