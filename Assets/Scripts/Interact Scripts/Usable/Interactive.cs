using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    // public variables
    public float outlineThickness = 0.5F;
    public bool pauseOnInteract = true;
    public string textId;
    public bool loopLast;
    public bool highlightEnds = false;

    // private trackers
    // outline vars
    private Material defaultMaterial;
    private Material outline;
    private SpriteRenderer sprite_renderer;

    // player tracker
    private bool near;

    // dialogue vars
    private InteractiveInfo[] interactivesText;
    private InteractiveUIController UI;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player" && OutlineEnabled())
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
        if(IsTriggered())
        {
            TriggerDialogue();
        }
    }

    public void Awake()
    {
        // get the objects for the outlines
        sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        defaultMaterial = sprite_renderer.material;

        // set default vars
        near = false;
    }

    public void Start()
    {
        UI = FindObjectOfType<InteractiveUIController>();
        outline.SetFloat("_Outline_Thickness", outlineThickness); 

        interactivesText = InteractiveInfo.ParseData(InteractiveTextDatabase.GetText(textId));
        if(interactivesText == null)
            interactivesText = new InteractiveInfo[0];
    }

    public bool IsPlayerNear()
    {
        return near;
    }

    public void SetOutline(Material outline)
    {
        this.outline = new Material(outline);
    }

    public Material GetOutline()
    {
        return outline;
    }

    void EnableOutline()
    {
        sprite_renderer.material = outline;
    }

    void DisableOutline()
    {
        sprite_renderer.material = defaultMaterial;
    }
    public void TriggerDialogue()
    {
        TriggerDialogue(textId, interactivesText, loopLast);

        if(!OutlineEnabled())
            DisableOutline();
    }

    public void TriggerDialogue(string script_id, InteractiveInfo[] allScripts, bool loop_last)
    {
        if(script_id == null) return;

        // don't trigger if dialogue is currently active
        if(UI.IsActive()) return;

        // pause now if I've made it this far

        int index = UIManager.GetInteractiveIndex(script_id);

        if(index >= allScripts.Length) return;

        if(pauseOnInteract) EntityManager.DialoguePause();
        UI.StartInteractive(allScripts[index], pauseOnInteract);

        if(!loop_last || index + 1 < allScripts.Length) index++;

        UIManager.SetInteractiveIndex(script_id, index);
    }

    bool OutlineEnabled()
    {
        int index = UIManager.GetInteractiveIndex(textId);

        return !highlightEnds || index < interactivesText.Length;
    }

    public bool IsTriggered()
    {
        return IsPlayerNear() && InputManager.InteractKeyDown();
    }

    public bool UIActive()
    {
        return UI.IsActive();
    }
}
