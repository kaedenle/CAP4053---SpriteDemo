using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : OutlineObject
{
    // public variables
    public bool pauseOnInteract = true;
    public InteractiveText interactiveText;
    public string normalAudio;
    public bool highlightEnds = false;

    public LockedBehavior lockable;

    // private trackers

    // dialogue vars
    // private InteractiveInfo[] interactivesText;
    private InteractiveUIController UI;
    private bool triggered = false;

    new protected void Awake()
    {
        base.Awake();
    }

    new protected void Start()
    {
        base.Start();

        UI = FindObjectOfType<InteractiveUIController>();

        interactiveText.SetText( GetText( interactiveText.GetID() ) );

        if(lockable.isLocked)
        {
            lockable.SetText(GetText(lockable.GetTextID()));
        }
    }

    protected string[][] GetText(string id)
    {
        return InteractiveTextDatabase.GetText(id);
    }

    new protected void OnTriggerEnter2D(Collider2D collider)
    {
        if(OutlineEnabled())
            base.OnTriggerEnter2D(collider);
    }

    new protected void Update()
    {
        base.Update();
        
        if(IsTriggered())
        {
            TriggerDialogue();

            if(lockable.IsUnlocked())
                triggered = true;
        }

        if(triggered && !UIActive())
        {
            ActivateBehaviors();
            triggered = false;
        }
    }

    protected virtual void ActivateBehaviors()
    {
        Debug.Log(normalAudio + " bozo");

        if(lockable.IsUnlocked())
        {
            SoundEffectManager.PlayAudio(normalAudio);
        }
        else
        {            
            lockable.PlayAudio();
        }
    }


    protected void TriggerDialogue()
    {
        if(lockable.IsUnlocked())
        {
            TriggerDialogue(interactiveText);

        }
        else
        {
            TriggerDialogue(lockable.GetInteractiveText());
            lockable.PlayAudio();
        }

        if(!OutlineEnabled())
            DisableOutline();
    }

    protected void TriggerDialogue(InteractiveText txt)
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

    protected bool IsTriggered()
    {
        return IsPlayerNear() && InputManager.InteractKeyDown();
    }

    protected bool UIActive()
    {
        if(UI == null) return false;

        return UI.IsActive();
    }
}
