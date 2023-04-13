using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour
{
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
        CastleSunglasses,
        Boss_Lever_Pulled,
        Hub_TalkedToKaleigh,
        Hub_TalkedToAntonio,
        Hub_TalkedToFinn,
        City_Paper1,
        City_Paper2,
        City_Paper3,
        City_Paper4,
        City_Paper5,
        City_Paper6,
        City_Paper7,
        City_Paper8
    }

    private static AllItems[] intangibles =
    {
        AllItems.Child_KnowledgeOf_Child,
        AllItems.Child_WorkingFusebox,
        AllItems.Boss_KeyUsedLeft,
        AllItems.Boss_KeyUsedDown,
        AllItems.Boss_KeyUsedRight,
        AllItems.Boss_Lever_Pulled,
        AllItems.Hub_TalkedToKaleigh,
        AllItems.Hub_TalkedToAntonio,
        AllItems.Hub_TalkedToFinn
    };

    // temporary solution to dropable heart problem
    // may cause issues later on when implementing an inventory UI
    private static AllItems[] repeatables =
    {
        AllItems.HealingHeart
    };

    public static List<AllItems> _inventoryItems = new List<AllItems>();  // our inventory items
    public static List<AllItems> _usedItems = new List<AllItems>();  // our inventory items
       
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

    public static List<AllItems> GetInventoryItems()
    {
        return new List<AllItems>( _inventoryItems );
    }

        public static List<AllItems> GetUsedItems()
    {
        return new List<AllItems> ( _usedItems );
    }

    public static void SetInventory(List<AllItems> ls)
    {
        _inventoryItems = new List<AllItems> (ls);
    }

    public static void SetUsed(List<AllItems> ls)
    {
        _usedItems = new List<AllItems> (ls);
    }
    
}
