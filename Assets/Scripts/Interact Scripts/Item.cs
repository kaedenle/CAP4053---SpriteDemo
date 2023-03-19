using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactive
{
    [SerializeField] InventoryManager.AllItems _itemType;

    void Start()
    {
        if (InventoryManager.PickedUp(_itemType))
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(IsPlayerNear() && InputManager.InteractKeyDown())
        {
            InventoryManager.AddItem(_itemType);
            Destroy(gameObject);
        }
    }
}
