using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public enum AllItems    // all available inventory items
    {
        MobsterKeyDemo,
        Child_Grabber,
        Child_Bedroom_Key,
        Child_Lightbulb,
        Child_Fuse,
        Child_Safe_Combo,
        Child_Plushie,
        Child_Crowbar,
        Child_Rope
    }

    public static List<AllItems> _inventoryItems = new List<AllItems>();  // our inventory items
    public static List<AllItems> _usedItems = new List<AllItems>();  // our inventory items

    public void Awake()
    {
        Instance = this;
    }
       
    // add item to current inventory
    public static void AddItem(AllItems item)
    {
        // make sure to only have one of each unique item in inventory
        if(!_inventoryItems.Contains(item))
        {
            _inventoryItems.Add(item);

            if(!_usedItems.Contains(item)) 
                _usedItems.Add(item);

            // debug statement for figuring out any issues
            Debug.Log("added " + item.ToString() + " to inventory");
            // Debug.Log("added " + item.ToString() + " to inventory, current inventory size is now " + _inventoryItems.Count);
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

    public static bool PickedUp(AllItems item)
    {
        return _usedItems.Contains(item);
    }

    public static void ResetVariables()
    {
        _inventoryItems.Clear();
        _usedItems.Clear();
    }
    
}
