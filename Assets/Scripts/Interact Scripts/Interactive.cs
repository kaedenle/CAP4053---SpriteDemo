using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    // public variables
    public float outline_thickness = 0.5F;
    public bool pause_on_interact = true;
    public bool loop_last;
    public InteractiveInfo[] interactivesText;

    // private trackers
    // outline vars
    private Material defaultMaterial;
    private Material outline;
    private SpriteRenderer sprite_renderer;

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
        sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        defaultMaterial = sprite_renderer.material;

        // set default vars
        near = false;
        interactive_index = 0;
        if(interactivesText == null)
            interactivesText = new InteractiveInfo[0];
    }

    public void Start()
    {
        UI = FindObjectOfType<InteractiveUIController>();
        // float k = 7.0F;
        // float scale = (gameObject.transform.localScale.x + gameObject.transform.localScale.y) / 2F;
        
        // Debug.Log("set thickness to " + (k / scale) + " where scale is " + scale + " and k is " + k);
        outline.SetFloat("_Outline_Thickness", outline_thickness); 
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
    void TriggerDialogue()
    {
        // don't trigger if dialogue is currently active
        if(UI.IsActive()) return;

        // don't trigger dialogue if you've already triggered the last one
        if(interactive_index >= interactivesText.Length) return;

        // pause now if I've made it this far
        if(pause_on_interact) EntityManager.DialoguePause();

        // do the dialogue
        UI.StartInteractive(interactivesText[interactive_index], pause_on_interact);
        if(!loop_last || interactive_index + 1 < interactivesText.Length) interactive_index++;
    }
}
