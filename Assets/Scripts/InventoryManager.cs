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

    public static List<AllItems> _inventoryItems = new List<AllItems>();  // our inventory items

    public void Awake()
    {
        Instance = this;

        // this should log at every scene switch
        Debug.Log("current inventory size: " + _inventoryItems.Count);
    }
       
    // add item to current inventory
    public static void AddItem(AllItems item)
    {
        // make sure to only have one of each unique item in inventory
        if(!_inventoryItems.Contains(item))
        {
            _inventoryItems.Add(item);

            // debug statement for figuring out any issues
            Debug.Log("added " + item.ToString() + " to inventory, current inventory size is now " + _inventoryItems.Count);
        }
    }

    // remove item from current inventory
    public static void RemoveItem(AllItems item)
    {
        // verify inventory actually contains item
        if (_inventoryItems.Contains(item))
        {
            _inventoryItems.Remove(item);

            // debug statements for figuring out any issues
            Debug.Log("removed " + item.ToString() + " from inventory, current inventory size is now " + _inventoryItems.Count);
        }
    }

    // returns if the player currently has the item in their inventory
    public static bool HasItem(AllItems item)
    {
        return _inventoryItems.Contains(item);
    }
    
}
