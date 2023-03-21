using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class InteractiveInfo
{
    // public TextAsset[] sentence_files;
    public string[] sentences;

    public InteractiveInfo(string[] text)
    {
        sentences = text;
    }

    public string[] GetSentences()
    {
        return sentences;
    }

    public static InteractiveInfo[] ParseData(string[][] text)
    {
        if(text == null) return new InteractiveInfo[0];

        InteractiveInfo[] ret = new InteractiveInfo[text.Length];

        for(int i = 0; i < text.Length; i++)
            ret[i] = new InteractiveInfo(text[i]);

        return ret;
    }
}
