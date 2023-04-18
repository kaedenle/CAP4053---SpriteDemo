using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemyManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static void Awaken()
    {
        GameObject EnemyStore = GameObject.Find("-- Enemies --");
        if (EnemyStore != null && AntonioManager.papers >= 8)
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
    void Awake()
    {
        Awaken();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
