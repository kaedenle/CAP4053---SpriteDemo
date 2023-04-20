using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterUI : MonoBehaviour
{
    
    public TMP_Text letterCounter;


    public void counterIncrement()
    {
        letterCounter.text = InventoryManager.counter.ToString();
    }
    
}
