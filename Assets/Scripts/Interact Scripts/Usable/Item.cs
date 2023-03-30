using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactive
{
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
        base.Update();

        if(ActivateBehavior())
        {
            InventoryManager.AddItem(_itemType);
            Destroy(gameObject);
        }


    }
}
