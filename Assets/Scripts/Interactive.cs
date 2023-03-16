using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Interactive
{
    public TextAsset[] sentence_files;
    private string[] sentences;

    public string[] GetSentences()
    {
        if(sentences == null)
        {        
            sentences = new string[sentence_files.Length];

            for(int i = 0; i < sentence_files.Length; i++)
            {
                sentences[i] = sentence_files[i].text;
            }
        }

        return sentences;
    }
}
