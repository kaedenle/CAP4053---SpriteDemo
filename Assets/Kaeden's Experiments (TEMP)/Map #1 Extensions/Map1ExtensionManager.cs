using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map1ExtensionManager : MonoBehaviour
{
    public static bool AntonioFlag = false;
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
        if(ItemStore != null && (AntonioManager.CheckPapers() == 0 || (AntonioManager.CheckPapers() > 8 && AntonioFlag)))
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
        if (AntiItemStore != null && (AntonioManager.CheckPapers() == 0 || (AntonioManager.CheckPapers() > 8) && AntonioFlag))
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
