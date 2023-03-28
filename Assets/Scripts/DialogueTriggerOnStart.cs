using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerOnStart : MonoBehaviour
{
    public NPCReport dialogue;
    public bool pauseWhenTriggered = true;
    private InteractiveUIController UI;

    // Start is called before the first frame update
    void Start()
    {
        UI = (InteractiveUIController) FindObjectOfType(typeof(InteractiveUIController));
    }

    // Update is called once per frame
    public void Update()
    {
        if(IsTriggered())
        {
            if(TriggerDialogue(dialogue))
                Destroy(gameObject);
        }
    }

    public bool TriggerDialogue(NPCReport conversation)
    {
        if(conversation == null) return true;

        string script_id = conversation.conversation_id;
        if(script_id == null) return true;

        // don't trigger if dialogue is currently active
        if(UIActive()) return false;

        int index = UIManager.GetInteractiveIndex(script_id);

        if(index >= conversation.Length()) return true;

        // pause now if I've made it this far
        if(pauseWhenTriggered) EntityManager.DialoguePause();
        UI.StartConversation(conversation.conversations[index]);

        index = conversation.CalculateNextIndex(index);
        UIManager.SetInteractiveIndex(script_id, index);

        return true;
    }

    public bool IsTriggered()
    {
        return EntityManager.IsUIInteractable();
    }

    public bool UIActive()
    {
        return UI.IsActive();
    }
}