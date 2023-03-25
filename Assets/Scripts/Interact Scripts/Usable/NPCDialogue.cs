using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : OutlineObject
{
    public NPCReport dialogue;
    public bool loopLast = false;
    public bool highlightEnds = true;

    private bool pauseOnInteract = true;

    private InteractiveUIController UI;

    // Start is called before the first frame update
    new public void Start()
    {
        base.Start();

        UI = (InteractiveUIController) FindObjectOfType(typeof(InteractiveUIController));
    }

    // Update is called once per frame
    public void Update()
    {

        if(IsTriggered())
        {
            TriggerDialogue();
        }
    }

    new public void OnTriggerEnter2D(Collider2D collider)
    {
        if(OutlineEnabled())
            base.OnTriggerEnter2D(collider);
    }

    public void TriggerDialogue()
    {
        if(dialogue == null) return;

        string script_id = dialogue.conversation_id;

        if(script_id == null) return;

        // don't trigger if dialogue is currently active
        if(UIActive()) return;

        int index = UIManager.GetInteractiveIndex(script_id);

        if(index >= dialogue.Length()) return;

        // pause now if I've made it this far
        if(pauseOnInteract) EntityManager.DialoguePause();
        UI.StartConversation(dialogue.conversations[index]);

        if(!loopLast || index + 1 < dialogue.Length()) index++;

        UIManager.SetInteractiveIndex(script_id, index);
        
        if(!OutlineEnabled())
            DisableOutline();
    }

    bool OutlineEnabled()
    {
        if(dialogue == null) return true;

        int index = UIManager.GetInteractiveIndex(dialogue.conversation_id);

        return !highlightEnds || index < dialogue.Length();
    }

    public bool IsTriggered()
    {
        return IsPlayerNear() && InputManager.InteractKeyDown();
    }

    public bool UIActive()
    {
        if(UI == null) Debug.Log("UI is null in NPCDialogue");
        return UI.IsActive();
    }
}
