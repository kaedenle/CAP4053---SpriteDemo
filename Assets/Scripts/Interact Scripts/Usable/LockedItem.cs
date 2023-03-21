using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedItem : Locked
{
    [SerializeField] public InventoryManager.AllItems itemType;

    new void Awake()
    {
        if(InventoryManager.PickedUp(itemType))
        {
            Destroy(gameObject);
        }

        base.Awake();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if(IsTriggered() && IsUnlocked())
        {
            Debug.Log("inside LockedItem Update()");
            InventoryManager.AddItem(itemType);
            Destroy(gameObject);
        }
    }
}
