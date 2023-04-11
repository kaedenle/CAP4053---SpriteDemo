using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// NOTE: 
// Will skip any dialogue if you pick up the item while triggering another interactable
// Not lockable!
public class PriorityDrops : Interactive
{
    [HideInInspector]
    public static event EventHandler PickedUp;
    [SerializeField] InventoryManager.AllItems _itemType;
    public bool repeatable;

    new void Start()
    {
        base.Start();

        if (InventoryManager.PickedUp(_itemType) && !repeatable)
        {
            Destroy(gameObject);
        }
    }

    new void Update()
    {
        if(IsTriggered())
        {
            if(!UIActive() && lockable.IsUnlocked())
            {
                TriggerDialogue();
            }
            
            Pickup();
        }
    }

    private void Pickup()
    {
        InventoryManager.AddItem(_itemType);

        if (PickedUp != null) 
            PickedUp(this, EventArgs.Empty);

        Destroy(gameObject);
    }
}
