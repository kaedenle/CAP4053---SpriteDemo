using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LockedDialogueBehavior
{
    public bool isLocked = false;
    public InventoryManager.AllItems[] requiredItems; 
    public NPCReport lockedText;

    private bool unlocked = false;
    
    public bool IsUnlocked()
    {
        if(!isLocked || requiredItems == null || unlocked) return true;

        foreach(InventoryManager.AllItems item in requiredItems)
        {
            if(!InventoryManager.PickedUp(item))
                return false;
        }

        return (unlocked = true);
    }

    public NPCReport GetDialogue()
    {
        return lockedText;
    }
}
