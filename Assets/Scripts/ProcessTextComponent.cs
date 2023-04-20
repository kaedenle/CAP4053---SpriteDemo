using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProcessTextComponent : MonoBehaviour
{
    // takes the text mesh text and checks if there are any key words/phrases to replace
    void Start()
    {
        Text txt = gameObject.GetComponent<Text>();
        if(txt != null) txt.text = Process(txt.text);

        TMP_Text proText = gameObject.GetComponent<TMP_Text>();
        if(proText != null) proText.text = Process(proText.text);
    }

    string Process(string txt)
    {
        for(int k = 0; k < InputManager.NUMBER_OF_KEYS; k++)
        {
            InputManager.Keys key = (InputManager.Keys) k;
            string pattern = "%" + key.ToString() + "%";
            // Debug.Log("checking for pattern: " + pattern);

            if(txt.Contains(pattern))
            {
                txt = txt.Replace(pattern, "\"" + InputManager.GetKeyString(key) + "\"");
            }
        }

        return txt;
    }
}
