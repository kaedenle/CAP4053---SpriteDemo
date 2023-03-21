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
        TriggerDialogue(lockedTextId, lockedText, true);
    }

    public bool IsUnlocked()
    {
        return InventoryManager.PickedUp(requiredItem);
    }

}
