using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderObjectByItem : MonoBehaviour
{
    public bool Despawn;
    public InventoryManager.AllItems[] Items;
    // Start is called before the first frame update
    void OnEnable()
    {
        InventoryManager.AddedItem += Execution;
    }
    void OnDisable()
    {
        InventoryManager.AddedItem -= Execution;
    }
    public void Execution(object sender, InventoryManager.AllItems e)
    {
        if (Items.Length == 0) return;
        int count = 0;
        foreach(InventoryManager.AllItems items in Items)
        {
            if (InventoryManager.HasItem(items)) count++;
        }
        if (count == Items.Length)
        {
            if (Despawn) gameObject.SetActive(false);
            else gameObject.SetActive(true);
        }
    }
    // Update is called once per frame
    void Start()
    {
        Execution(null, InventoryManager.AllItems.City_Paper1);
    }
}
