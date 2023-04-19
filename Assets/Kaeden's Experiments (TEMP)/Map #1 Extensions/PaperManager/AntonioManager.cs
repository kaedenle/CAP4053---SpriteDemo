using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AntonioManager : MonoBehaviour
{
    //will check to see if you picked up paper
    private static HashSet<InventoryManager.AllItems> checkPaper = new HashSet<InventoryManager.AllItems>() { InventoryManager.AllItems.City_Paper1, InventoryManager.AllItems.City_Paper2, 
        InventoryManager.AllItems.City_Paper3, InventoryManager.AllItems.City_Paper4, InventoryManager.AllItems.City_Paper5, InventoryManager.AllItems.City_Paper6, InventoryManager.AllItems.City_Paper7, 
        InventoryManager.AllItems.City_Paper8 };
    public static int CheckPapers()
    {
        int ret = 0;
        foreach(InventoryManager.AllItems p in checkPaper)
            if (InventoryManager.HasItem(p)) ret++;
        return ret;
    }
    public static bool IsPage(InventoryManager.AllItems i)
    {
        return checkPaper.Contains(i);
    }
}
