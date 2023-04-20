using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : Interactive
{
    [HideInInspector]
    public static event EventHandler<InventoryManager.AllItems> PickedUp;
    [SerializeField] public InventoryManager.AllItems _itemType;
    public bool repeatable;
    public bool destroyOnInteract = true;

    new void Start()
    {
        base.Start();

        if (InventoryManager.PickedUp(_itemType) && !repeatable)
        {
            if(destroyOnInteract)
                Destroy(gameObject);
        }
    }

    protected override void ActivateBehaviors()
    {
        base.ActivateBehaviors();
        Pickup();
    }

    private void Pickup()
    {
        InventoryManager.AddItem(_itemType);

        if (PickedUp != null) 
            PickedUp(this, _itemType);
        
        if(destroyOnInteract)
            Destroy(gameObject);
    }

}
