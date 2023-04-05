using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : OutlineObject
{
    // public variables
    public bool pauseOnInteract = true;
    public InteractiveText interactiveText;
    public bool highlightEnds = false;

    public LockedBehavior lockable;

    // private trackers

    // dialogue vars
    // private InteractiveInfo[] interactivesText;
    private InteractiveUIController UI;
    private bool triggered = false;

    new public void Awake()
    {
        base.Awake();
    }

    new public void Start()
    {
        base.Start();

        UI = FindObjectOfType<InteractiveUIController>();

        interactiveText.SetText( GetText( interactiveText.GetID() ) );

        if(lockable.isLocked)
        {
            lockable.SetText(GetText(lockable.GetTextID()));
        }
    }

    public string[][] GetText(string id)
    {
        return InteractiveTextDatabase.GetText(id);
    }

    new public void OnTriggerEnter2D(Collider2D collider)
    {
        if(OutlineEnabled())
            base.OnTriggerEnter2D(collider);
    }

    public void Update()
    {
        base.Update();
        
        if(IsTriggered())
        {
            if(!UIActive() && lockable.IsUnlocked()) triggered = true;
            TriggerDialogue();
        }
    }


    public void TriggerDialogue()
    {
        if(lockable.IsUnlocked())
            TriggerDialogue(interactiveText);
        else
        {
            TriggerDialogue(lockable.GetInteractiveText());
        }

        if(!OutlineEnabled())
            DisableOutline();
    }

    public void TriggerDialogue(InteractiveText txt)
    {
        if(txt.IsEmpty()) return;

        // don't trigger if dialogue is currently active
        if(UI.IsActive()) return;

        int index = UIManager.GetInteractiveIndex(txt.GetID());

        if(txt.OutOfBounds(index)) return;

        // pause now if I've made it this far
        UI.StartInteractive(txt.GetUnit(index), pauseOnInteract);

        index = txt.CalcNextIndex(index);

        UIManager.SetInteractiveIndex(txt.GetID(), index);
    }

    bool OutlineEnabled()
    {
        if(interactiveText.IsEmpty()) return true;

        int index = UIManager.GetInteractiveIndex(interactiveText.GetID());

        return !highlightEnds || index < interactiveText.Length() || !lockable.IsUnlocked();
    }

    public bool IsTriggered()
    {
        return IsPlayerNear() && InputManager.InteractKeyDown();
    }

    public bool UIActive()
    {
        if(UI == null) return false;

        return UI.IsActive();
    }

    public bool ActivateBehavior()
    {
        if(!triggered || UIActive() || !lockable.IsUnlocked()) return false;

        triggered = false;
        return true;
    }
}
