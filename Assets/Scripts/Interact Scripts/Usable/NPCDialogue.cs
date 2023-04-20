using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPCDialogue : OutlineObject
{
    public NPCReport dialogue;
    public string normalAudio;
    public bool highlightEnds = true;
    public LockedDialogueBehavior lockable;

    public bool playAfterDialogue=true;

    public static event EventHandler Talked;
    private InteractiveUIController UI;
    private bool triggered = false;

    // Start is called before the first frame update
    new public void Start()
    {
        base.Start();

        UI = (InteractiveUIController) FindObjectOfType(typeof(InteractiveUIController));
    }

    // Update is called once per frame
    new public void Update()
    {
        base.Update();

        if(IsTriggered())
        {
            if(!UIActive() && lockable.IsUnlocked())
            {
                InitiateBehaviors();

            }
            TriggerDialogue();
        }
    }

    public void InitiateBehaviors()
    {
        triggered = true;

        if(lockable.IsUnlocked())
        {
            if(playAfterDialogue)
            {
                SoundEffectManager.PlayAudio(normalAudio);
            }
        }
        else
            lockable.PlayAudio();
    }

    new public void OnTriggerEnter2D(Collider2D collider)
    {
        if(OutlineEnabled())
            base.OnTriggerEnter2D(collider);
    }

    public void TriggerDialogue()
    {
        if(lockable.IsUnlocked())
        {
            if(!playAfterDialogue)
            {
                SoundEffectManager.PlayAudio(normalAudio);
            }
            TriggerDialogue(dialogue);
            
        }
        else   
            TriggerDialogue(lockable.GetDialogue());

        if(!OutlineEnabled())
            DisableOutline();
    }

    public void TriggerDialogue(NPCReport conversation)
    {
        if(conversation == null) return;

        string script_id = conversation.conversation_id;

        if(script_id == null) return;

        // don't trigger if dialogue is currently active
        if(UIActive()) return;

        int index = UIManager.GetInteractiveIndex(script_id);

        if(index >= conversation.Length()) return;

        UI.StartConversation(conversation.conversations[index]);

        index = conversation.CalculateNextIndex(index);
        UIManager.SetInteractiveIndex(script_id, index);
        if (Talked != null) Talked(gameObject, EventArgs.Empty);
    }

    bool OutlineEnabled()
    {
        if(dialogue == null || !lockable.IsUnlocked()) return true;

        int index = UIManager.GetInteractiveIndex(dialogue.conversation_id);

        return !highlightEnds || index < dialogue.Length();
    }

    public bool IsTriggered()
    {
        return IsPlayerNear() && InputManager.InteractKeyDown();
    }

    public bool UIActive()
    {
        if(UI == null) Debug.Log("UI is null in NPCDialogue");
        return UI.IsActive();
    }

     public bool ActivateBehavior()
    {
        if(!triggered || UIActive() || !lockable.IsUnlocked()) return false;

        triggered = false;
        return true;
    }
}
