using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locked : Interactive
{
    [SerializeField]
    public InventoryManager.AllItems requiredItem;
    public string lockedTextId;

    private InteractiveText lockedText;
    private int locked_index;

    new void Start()
    {
        lockedText.textID = lockedTextId;
        lockedText.loopLast = true;
        
        base.Start();
        lockedText.SetText( InteractiveTextDatabase.GetText(lockedTextId) );
    }

    new public void Update()
    {
        if(IsTriggered())
        {
            bool unlocked = IsUnlocked();

            if(unlocked)
                TriggerDialogue();
            else
                TriggerLockedDialogue();
        }
    }

    void TriggerLockedDialogue()
    {
        TriggerDialogue(lockedText);
    }

    public bool IsUnlocked()
    {
        return InventoryManager.PickedUp(requiredItem);
    }

}
