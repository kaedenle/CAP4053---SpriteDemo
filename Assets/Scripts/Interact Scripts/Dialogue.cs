using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string[] dialogue;
    public string[] names;
    
    
    public string[] GetSentences()
    {
        return dialogue;
    }

    public string[] GetNames()
    {
        return names;
    }
}
