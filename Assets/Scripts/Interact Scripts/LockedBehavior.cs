using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LockedBehavior
{
    public bool isLocked = false;
    public string lockedTextID;
    public bool loopLast = true;
    public InventoryManager.AllItems[] requiredItems; 

    private bool unlocked = false;
    private InteractiveInfo[] lockedText;
    
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

    public void SetText(InteractiveInfo[] text)
    {
        lockedText = text;
        if(text == null)
            lockedText = new InteractiveInfo[0];
    }

    public InteractiveInfo[] GetText()
    {
        return lockedText;
    }
}
