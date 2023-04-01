using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : Interactive
{
    [HideInInspector]
    public static event EventHandler PickedUp;
    [SerializeField] InventoryManager.AllItems _itemType;
    public bool repeatable;
    private void Pickup()
    {
        InventoryManager.AddItem(_itemType);
        if (PickedUp != null) PickedUp(this, EventArgs.Empty);
        Destroy(gameObject);
    }

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
        base.Update();

        if(ActivateBehavior())
        {
            Pickup();
        }


    }
}
