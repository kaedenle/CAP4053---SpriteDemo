using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System;
using Newtonsoft.Json;

public class InteractiveTextDatabase : MonoBehaviour
{
    // provide the database
    public TextAsset interactiveTextJSON;
    public static InteractiveTextDatabase Instance;
    private static Dictionary<string, string[][]> interact_data;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
        }

        Instance = this;

        if(interact_data == null)
        {
            interact_data = new Dictionary<string, string[][]>();
            GrabDatabase();
        }
    }

    void GrabDatabase()
    {
        JsonTextReader reader = InitReader();

        while(reader.Read() && reader.TokenType != JsonToken.EndObject)
        {
            ReadInteractive(reader);
        }
    }

    JsonTextReader InitReader()
    {
        JsonTextReader reader = new JsonTextReader(new StringReader(interactiveTextJSON.text));
        reader.Read();
        return reader;
    }

    void ReadInteractive(JsonTextReader reader)
    {
        string id = (string) reader.Value;
        List<List<string>> dialogue = new List<List<string>>();

        while(reader.Read() && reader.TokenType != JsonToken.EndArray)
        {
            List<string> cur = new List<string>();

            while(reader.Read() && reader.TokenType != JsonToken.EndArray)
            {
                if(reader.Value == null) continue;

                cur.Add((string)reader.Value);
            }

            dialogue.Add(cur);
        }

        string[][] words = new string[dialogue.Count][];

        // Debug.Log("Interactive: " + id);
        for(int i = 0 ;i < dialogue.Count; i++)
        {
            words[i] = new string[dialogue[i].Count];
            for(int j = 0; j < dialogue[i].Count; j++)
                words[i][j] = dialogue[i][j];
        }

        interact_data.Add(id, words);
    }

    public static string[][] GetText(string id)
    {
        if(!interact_data.ContainsKey(id)) return null;

        return interact_data[id];
    }
}
