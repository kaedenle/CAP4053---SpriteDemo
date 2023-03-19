using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    [SerializeField] InventoryManager.AllItems _keyType;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            InventoryManager.AddItem(_keyType);
            Destroy(gameObject);
        }
    }

    void Awake()
    {
        if (MobsterLevelManager.usedKey(_keyType) || InventoryManager.HasItem(_keyType))
        {
            Destroy(gameObject);
        }
    }

}
