using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterUI : MonoBehaviour
{
    public TMP_Text letterCounter;
    private InventoryManager.AllItems[] letters =
    {
        InventoryManager.AllItems.Letter1,
        InventoryManager.AllItems.Letter2,
        InventoryManager.AllItems.Letter3,
        InventoryManager.AllItems.Letter4
    };

    void Awake()
    {
        counterIncrement();
    }

    public void counterIncrement()
    {
        letterCounter.text = CountLetters().ToString();
    }

    public int CountLetters()
    {
        int total = 0;
        for(int i = 0; i < letters.Length; i++)
            if(InventoryManager.PickedUp(letters[i]))
                total++;
        return total;
    }
    
}
