using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractiveText
{
    public string textID;
    public bool loopLast = true;
    private string[][] sentences;

    public void SetText(string[][] txt)
    {
        sentences = txt;
    }    

    public string GetID()
    {
        return textID;
    }

    public bool IsEmpty()
    {
        return (textID == null || sentences == null || textID.Trim().Equals(""));
    }

    public bool OutOfBounds(int index)
    {
        return textID == null || sentences == null || index >= sentences.Length;
    }

    public string[] GetUnit(int index)
    {
        if(sentences == null || index >= sentences.Length) return null;
        return sentences[index];
    }

    public int CalcNextIndex(int index)
    {
        if(sentences == null) return index;

        if(!loopLast || index + 1 < sentences.Length) return index + 1;

        return index;
    }

    public int Length()
    {
        if(sentences == null) return 0;
        return sentences.Length;
    }
}
