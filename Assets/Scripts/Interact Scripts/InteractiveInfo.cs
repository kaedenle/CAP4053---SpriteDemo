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
        // if(sentences == null)
        // {        
        //     sentences = new string[sentence_files.Length];

        //     for(int i = 0; i < sentence_files.Length; i++)
        //     {
        //         sentences[i] = sentence_files[i].text;
        //     }
        // }

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
