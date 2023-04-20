using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterUI : MonoBehaviour
{
    
    [SerializeField] public InventoryManager.AllItems letter;
    public TMP_Text letterCounter;
    public int counter = 0;

    private void Update() 
    {
        letterCounter.text = InventoryManager.counter.ToString();
    }
    
}
