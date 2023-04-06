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
        Child_Rope,
        Child_KnowledgeOf_Child,
        Child_WorkingFusebox,
        Boss_KeyLeft,
        Boss_KeyDown,
        Boss_KeyRight,
        Boss_KeyUsedLeft,
        Boss_KeyUsedDown,
        Boss_KeyUsedRight,
        HealingHeart,
        CastleChestKey,
        CastleDagger,
        CastleSunglasses
    }

    private static AllItems[] intangibles =
    {
        AllItems.Child_KnowledgeOf_Child,
        AllItems.Child_WorkingFusebox,
        AllItems.Boss_KeyUsedLeft,
        AllItems.Boss_KeyUsedDown,
        AllItems.Boss_KeyUsedRight
    };

    // temporary solution to dropable heart problem
    // may cause issues later on when implementing an inventory UI
    private static AllItems[] repeatables =
    {
        AllItems.HealingHeart
    };

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
        if(!_inventoryItems.Contains(item) || !IsUnique(item))
        {
            if(!IsIntangible(item))
                _inventoryItems.Add(item);

            if(!_usedItems.Contains(item)) 
                _usedItems.Add(item);

            // debug statement for figuring out any issues
            Debug.Log("added " + item.ToString() + " to inventory");
            // Debug.Log("added " + item.ToString() + " to inventory, current inventory size is now " + _inventoryItems.Count);
        }
    }

    public static bool IsUnique(AllItems item)
    {
        return Array.IndexOf(repeatables, item) < 0;
    }

    public static bool IsIntangible(AllItems item)
    {
        foreach(AllItems t in intangibles)
            if(t == item)
                return true;
        return false;
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
