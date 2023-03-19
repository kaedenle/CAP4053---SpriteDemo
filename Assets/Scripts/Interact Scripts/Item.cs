using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactive
{
    [SerializeField] InventoryManager.AllItems _itemType;

    new void Start()
    {
        base.Start();

        if (InventoryManager.PickedUp(_itemType))
        {
            Destroy(gameObject);
        }
    }

    new void Update()
    {
        if(IsPlayerNear() && InputManager.InteractKeyDown())
        {
            InventoryManager.AddItem(_itemType);
            base.Update();
            Destroy(gameObject);
        }

        else
        {
            base.Update();
        }

    }
}
