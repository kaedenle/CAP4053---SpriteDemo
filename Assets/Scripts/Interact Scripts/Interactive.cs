using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    // public variables
    public bool loop_last;
    public InteractiveInfo[] interactives;

    // private trackers
    // outline vars
    private Material defaultMaterial;
    private Material outline;
    private SpriteRenderer renderer;

    // player tracker
    private bool near;

    // dialogue vars
    private int interactive_index;
    private InteractiveUIController UI;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            near = true;
            EnableOutline();
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            near = false;
            DisableOutline();
        }
    }

    public void Update()
    {
        if(IsPlayerNear() && InputManager.InteractKeyDown())
        {
            TriggerDialogue();
        }
    }

    void Awake()
    {
        // get the objects for the outlines
        renderer = gameObject.GetComponent<SpriteRenderer>();
        defaultMaterial = renderer.material;

        // set default vars
        near = false;
        interactive_index = 0;
        UI = FindObjectOfType<InteractiveUIController>();
    }

    public bool IsPlayerNear()
    {
        return near;
    }

    public void SetOutline(Material outline)
    {
        this.outline = outline;
    }

    void EnableOutline()
    {
        renderer.material = outline;
    }

    void DisableOutline()
    {
        renderer.material = defaultMaterial;
    }
    void TriggerDialogue()
    {
        // don't trigger if dialogue is currently active
        if(!UI.IsActive()) return;

        // don't trigger dialogue if you've already triggered the last one
        if(interactive_index <= interactives.Length) return;

        // do the dialogue
        UI.StartInteractive((interactives[interactive_index]));
        if(!loop_last || interactive_index + 1 < interactives.Length) interactive_index++;

    }
}
