using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LockedBehavior
{
    public bool isLocked = false;
    public InteractiveText lockedInteractiveText;
    public string lockedAudio;
    public InventoryManager.AllItems[] requiredItems; 

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

    public void SetText(string[][] txt)
    {
        lockedInteractiveText.SetText(txt);
    }

    public string GetTextID()
    {
        return lockedInteractiveText.GetID();
    }

    public InteractiveText GetInteractiveText()
    {
        return lockedInteractiveText;
    }

    public void PlayAudio()
    {
        SoundEffectManager.PlayAudio(lockedAudio);
    }
}
