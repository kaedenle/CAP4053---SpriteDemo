using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private DialogueManager Instance;
    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();
    }
}
