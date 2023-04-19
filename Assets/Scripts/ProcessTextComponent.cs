using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessTextComponent : MonoBehaviour
{
    // takes the text mesh text and checks if there are any key words/phrases to replace
    void Start()
    {
        Text tmp = gameObject.GetComponent<Text>();
        string txt = tmp.text;

        if(tmp == null)
        {
            Debug.LogError("processor script found no Text to replace");
            return;
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

        tmp.text = txt;
    }
}
