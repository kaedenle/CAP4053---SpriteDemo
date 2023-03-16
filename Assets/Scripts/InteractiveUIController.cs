using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractiveUIController : MonoBehaviour
{
    private Queue<string> sentences;
    private bool on;
    public TMP_Text textField;
    UnityEngine.EventSystems.UIBehaviour[] allUI;

    // Start is called before the first frame update
    void Start()
    {
        allUI = gameObject.GetComponentsInChildren<UnityEngine.EventSystems.UIBehaviour>(); 
        TurnUIOff();
        sentences = new Queue<string>();
        on = false;
    }

    // check if the interactive needs to be triggered
    void Update()
    {
        if(on && InputManager.ContinueKeyPressed())
        {
            DisplayNextSentence();
        }
    }

    // start the interactive
    public void StartInteractive(Interactive interactive)
    {
        on = true;
        sentences.Clear();
        foreach (string sentence in interactive.GetSentences())
        {
            sentences.Enqueue(sentence);
        }

        TurnUIOn();
        DisplayNextSentence();
    }

    // display the next sentence
    void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndInteractive();
            return;
        }

        string sentence = sentences.Dequeue();
        textField.text = sentence;
        Debug.Log("sentence: " + sentence);
    }

    // returns if this UI is being used
    public bool IsActive()
    {
        return on;
    }

    // end this interactive
    void EndInteractive()
    {
        Debug.Log("Ending Interactive...");
        TurnUIOff();
        // wait some # of seconds
        on = false;
    }

    void TurnUIOff()
    {
        foreach(UnityEngine.EventSystems.UIBehaviour ui in allUI) 
        { 
            ui.enabled = false; //turn them off. 
        } 
    }

    void TurnUIOn()
    {
        foreach(UnityEngine.EventSystems.UIBehaviour ui in allUI) 
        { 
            ui.enabled = true; //turn them off. 
        } 
    }
}
