using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public enum AllItems    // all available inventory items
    {
        MobsterKeyDemo
    }

    public List<AllItems> _inventoryItems = new List<AllItems>();  // our inventory items

    public void Awake()
    {
        Instance = this;
    }
       
    // add item to current inventory
    public void AddItem(AllItems item)
    {
        // make sure to only have one of each unique item in inventory
        if(!_inventoryItems.Contains(item))
        {
            _inventoryItems.Add(item);
        }
    }

    // remove item from current inventory
    public void RemoveItem(AllItems item)
    {
        // verify inventory actually contains item
        if (_inventoryItems.Contains(item))
        {
            _inventoryItems.Remove(item);
        }
    }

    // returns if the player currently has the item in their inventory
    public bool HasItem(AllItems item)
    {
        return _inventoryItems.Contains(item);
    }
    
}
