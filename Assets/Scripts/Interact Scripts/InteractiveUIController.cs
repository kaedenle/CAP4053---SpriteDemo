using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractiveUIController : MonoBehaviour
{
    // public variables
    public TMP_Text textField;
    public GameObject nameBox;

    // internal variables
    private Queue<string> sentences;
    private Queue<string> names;
    private Queue<Dialogue> conversation;
    private Canvas canvas_renderer;
    private bool on_using;
    private bool on;
    private float delay = 0.5F;
    private float react_delay = 0.25F;
    private bool delay_complete = false;
    private bool pause = true;
    private TMP_Text nameField;
    private bool isDialogue = false;

    // Start is called before the first frame update
    void Start()
    {
        canvas_renderer = gameObject.GetComponent<Canvas>(); 
        TurnUIOff();
        sentences = new Queue<string>();
        names = new Queue<string>();
        on = false;
        on_using = false;

        if(nameBox != null)
            nameField = nameBox.GetComponentInChildren<TMP_Text>();
    }

    // check if the interactive needs to be triggered
    void Update()
    {
        if(on_using && delay_complete && InputManager.ContinueKeyPressed())
        {
            DisplayNextSentence();
        }
    }

    // start the interactive
    public void StartInteractive(string[] interactive, bool pause)
    {
        delay_complete = false;
        on = true;
        on_using = true;

        StartCoroutine(ReactDelay());

        if(this.pause = pause)
            EntityManager.DialoguePause();
        
        isDialogue = false;

        sentences.Clear();
        foreach (string sentence in interactive)
        {
            sentences.Enqueue(sentence);
        }

        TurnUIOn();
        DisplayNextSentence();
    }

    IEnumerator ReactDelay()
    {
        yield return new WaitForSecondsRealtime(react_delay);
        delay_complete = true;
    }

    public void StartConversation(Conversation converse)
    {
        on = true;
        on_using = true;

        if(!EntityManager.IsPaused()) 
        {
            EntityManager.DialoguePause();
            pause = true;
        }

        isDialogue = true;
        sentences.Clear();
        names.Clear();

        foreach(Dialogue dialogue in converse.dialogue)
        {
            sentences.Enqueue(dialogue.words);
            names.Enqueue(NPCManager.GetName(dialogue.speaker));

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

        delay_complete = false;
        StartCoroutine(ReactDelay());

        if(isDialogue)
        {
            string name = names.Dequeue();
            nameField.text = name;
        }

        string sentence = sentences.Dequeue();
        textField.text = sentence;
        // Debug.Log("sentence: " + sentence);
    }

    // returns if this UI is being used
    public bool IsActive()
    {
        return on || on_using;
    }

    // end this interactive
    void EndInteractive()
    {
        // Debug.Log("Ending Interactive...");
        on_using = false;
        TurnUIOff();
        StartCoroutine(WaitAndTurnOff());
    }
    IEnumerator WaitAndTurnOff()
    {
        //Wait for <delay> seconds
        yield return new WaitForSecondsRealtime(delay);
        on = false;
        if(pause)
            EntityManager.Unpause();
    }

    void TurnUIOff()
    {
        canvas_renderer.enabled = false; //turn them off. 
    }

    void TurnUIOn()
    {
        canvas_renderer.enabled = true; //turn it on 

        if(nameBox != null)
        {
            if(isDialogue)
            {
                nameBox.SetActive(true);
            }

            else
            {
                nameBox.SetActive(false);
            }
        }
    }
}
