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
    public string[] requiredEvents;

    private bool unlocked = false;
    
    public bool IsUnlocked()
    {
        if(!isLocked || (requiredItems == null && requiredEvents == null) || unlocked) return true;

        if(requiredItems != null)
            foreach(InventoryManager.AllItems item in requiredItems)
            {
                if(!InventoryManager.PickedUp(item))
                    return false;
            }
        
        if(requiredEvents != null)
            foreach(string e in requiredEvents)
            {
                if(!LevelManager.GetInteractiveState(e))
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
