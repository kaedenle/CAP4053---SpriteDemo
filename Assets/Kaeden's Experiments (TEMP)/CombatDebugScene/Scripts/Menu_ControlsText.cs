using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_ControlsText : MonoBehaviour
{
    public string[] texts;
    public InputManager.Keys[] reference;
    public InputManager.Keys[] secondaryReference;
    // Start is called before the first frame update
    private void OnEnable()
    {
        if (reference.Length == 0) return;
        int counter = 0;
        int count2 = 0;
        string text = "";
        foreach(string s in texts)
        {
            string m = s;
            while (m.IndexOf("*") > 0)
            {
                if (reference.Length == 0) return;
                m = ReplaceFirst(m, "*", InputManager.GetKeyString(reference[counter]));
                counter = (counter + 1) % reference.Length;
            }
            while (m.IndexOf("#") > 0)
            {
                if (secondaryReference.Length == 0) return;
                string replace = InputManager.HasSecondary(secondaryReference[count2]) ? " or " +InputManager.GetSecondaryKeyString(secondaryReference[count2]) : "";
                m = ReplaceFirst(m, "#", replace);
                count2 = (count2 + 1) % secondaryReference.Length;
            }
            text += m;
            text += "\n";
        }
        
        
        Text[] child = GetComponentsInChildren<Text>();
        foreach(Text t in child)
        {
            t.text = text;
        }
    }
    private string ReplaceFirst(string text, string search, string replace)
    {
        int pos = text.IndexOf(search);
        if (pos < 0)
        {
            return text;
        }
        return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
