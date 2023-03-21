using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locked : Interactive
{
    [SerializeField]
    public InventoryManager.AllItems requiredItem;
    public string lockedTextId;

    private InteractiveInfo[] lockedText;
    private int locked_index;

    new void Start()
    {
        base.Start();
        lockedText = InteractiveInfo.ParseData(InteractiveTextDatabase.GetText(lockedTextId));
    }

    new void Update()
    {
        if(IsPlayerNear() &&  InputManager.InteractKeyDown())
        {
            bool unlocked = InventoryManager.PickedUp(requiredItem);

            if(unlocked)
                TriggerDialogue();
            else
                TriggerLockedDialogue();
        }
    }

    void TriggerLockedDialogue()
    {
        // don't trigger dialogue if you've already triggered the last one
        if(locked_index >= lockedText.Length) return;

        // do the dialogue
        TriggerDialogue(lockedText[locked_index]);
        
        // default behavior: loop last
        if(locked_index + 1 < lockedText.Length) locked_index++;
    }

}
