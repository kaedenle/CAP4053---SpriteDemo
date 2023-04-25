using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
Key
%Keyname% -> "Keycode1"
%KeynameOption% -> "Keycode1" [or "Keycode2"]
*/

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
        // try for the Option (returns "Option1" or "Option2" OR "Option1")
        for(int k = 0; k < InputManager.NUMBER_OF_KEYS; k++)
        {
            InputManager.Keys key = (InputManager.Keys) k;
            string pattern = "%" + key.ToString() + "Option%";
            // Debug.Log("checking for pattern: " + pattern);

            if(txt.Contains(pattern))
            {
                string replace = "\"" + InputManager.GetKeyString(key) + "\"";
                if(InputManager.HasSecondary(key)) replace += " or \"" + InputManager.GetSecondaryKeyString(key) + "\"";
                txt = txt.Replace(pattern, replace);
            }
        }


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
