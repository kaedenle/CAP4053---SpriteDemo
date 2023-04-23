using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterCounter : MonoBehaviour
{
    private TMP_Text counter;
    private InventoryManager.AllItems[] letters =
    {
        InventoryManager.AllItems.Letter1,
        InventoryManager.AllItems.Letter2,
        InventoryManager.AllItems.Letter3,
        InventoryManager.AllItems.Letter4
    };
    void Awake()
    {
        counter = GetComponent<TMP_Text>();


    }
}
