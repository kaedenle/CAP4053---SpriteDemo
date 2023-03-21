using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedItem : Locked
{
    public InventoryManager.AllItems itemType;

    new void Awake()
    {
        if(InventoryManager.PickedUp(itemType))
        {
            Destroy(this);
        }

        base.Awake();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if(IsTriggered() && IsUnlocked())
        {
            InventoryManager.AddItem(itemType);
            Destroy(this);
        }
    }
}
