using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBehavior : MonoBehaviour
{
    [SerializeField] InventoryManager.AllItems _itemType;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            InventoryManager.Instance.AddItem(_itemType);
            Destroy(gameObject);
        }
    }
}
