using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractiveUIController : MonoBehaviour
{
    public TMP_Text textField;
    private Queue<string> sentences;
    private UnityEngine.EventSystems.UIBehaviour[] allUI;
    private bool on_using;
    private bool on;
    private float delay = 0.5F;

    // Start is called before the first frame update
    void Start()
    {
        allUI = gameObject.GetComponentsInChildren<UnityEngine.EventSystems.UIBehaviour>(); 
        TurnUIOff();
        sentences = new Queue<string>();
        on = false;
        on_using = false;
    }

    // check if the interactive needs to be triggered
    void Update()
    {
        if(on_using && InputManager.ContinueKeyPressed())
        {
            DisplayNextSentence();
        }
    }

    // start the interactive
    public void StartInteractive(Interactive interactive)
    {
        on = true;
        on_using = true;
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
        return on || on_using;
    }

    // end this interactive
    void EndInteractive()
    {
        Debug.Log("Ending Interactive...");
        on_using = false;
        TurnUIOff();
        StartCoroutine(WaitAndTurnOff());
    }
    IEnumerator WaitAndTurnOff()
    {
        //Wait for 4 seconds
        yield return new WaitForSeconds(delay);
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
