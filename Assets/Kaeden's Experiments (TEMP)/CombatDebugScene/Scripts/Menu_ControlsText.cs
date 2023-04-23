using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_ControlsText : MonoBehaviour
{
    public string[] texts;
    public InputManager.Keys[] reference;
    // Start is called before the first frame update
    private void OnEnable()
    {
        if (reference.Length == 0) return;
        int counter = 0;
        string text = "";
        foreach(string s in texts)
        {
            string m = s;
            while (m.IndexOf("*") > 0)
            {
                m = ReplaceFirst(m, "*", InputManager.GetKeyString(reference[counter]));
                counter = (counter + 1) % reference.Length;
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
