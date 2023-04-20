using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map1ExtensionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static void Awaken()
    {
        GameObject EnemyStore = GameObject.Find("-- Enemies --");
        if (EnemyStore != null && AntonioManager.CheckPapers() >= 8)
        {
            foreach (Transform child in EnemyStore.transform)
                child.gameObject.SetActive(false);
        }
        else if (EnemyStore != null && InventoryManager.HasItem(InventoryManager.AllItems.City_Paper1))
        {
            foreach (Transform child in EnemyStore.transform)
                child.gameObject.SetActive(true);
        }
    }
    public static void SpawnItems()
    {
        GameObject ItemStore = GameObject.Find("-- Collect --");
        if(ItemStore != null && (AntonioManager.CheckPapers() == 0))
        {
            foreach (Transform child in ItemStore.transform)
                child.gameObject.SetActive(false);
        }
        else if (ItemStore != null && AntonioManager.CheckPapers() > 0)
        {
            foreach (Transform child in ItemStore.transform)
                child.gameObject.SetActive(true);
        }
    }
    public static void AntispawnItems()
    {
        GameObject AntiItemStore = GameObject.Find("-- AntiCollect --");
        if (AntiItemStore != null && (AntonioManager.CheckPapers() == 0))
        {
            foreach (Transform child in AntiItemStore.transform)
                child.gameObject.SetActive(true);
        }
        else if (AntiItemStore != null && AntonioManager.CheckPapers() > 0)
        {
            foreach (Transform child in AntiItemStore.transform)
                child.gameObject.SetActive(false);
        }
    }
    public static void DoorWay()
    {
        GameObject DoorStore = GameObject.Find("-- FinalDoor --");
        GameObject Door2Store = GameObject.Find("-- FinalDoorAfter --");
        if (!InventoryManager.HasItem(InventoryManager.AllItems.City_Syringe)) return;
        if (DoorStore == null || Door2Store == null) return;
        //actual door
        foreach (Transform child in DoorStore.transform)
        {
            child.gameObject.SetActive(false);
        }
        //dialouge door
        foreach (Transform child in Door2Store.transform)
        {
            child.gameObject.SetActive(true);
        }
    }
    public void CallItems()
    {
        SpawnItems();
        AntispawnItems();
    }
    void Awake()
    {
        Awaken();
        SpawnItems();
        AntispawnItems();
        DoorWay();
    }
}
