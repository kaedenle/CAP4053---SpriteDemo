using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineObject : MonoBehaviour
{
    public float outlineThickness = 0.5F;
    private Material defaultMaterial;
    private Material outline;
    private SpriteRenderer sprite_renderer;

    private bool near = false;

    public void Awake()
    {
        // get the objects for the outlines
        sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        defaultMaterial = sprite_renderer.material;
    }

    public void Start()
    {
        if(outline != null) outline.SetFloat("_Outline_Thickness", outlineThickness); 
    }

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

    public void EnableOutline()
    {
        sprite_renderer.material = outline;
    }

    public void DisableOutline()
    {
        sprite_renderer.material = defaultMaterial;
    }

    public void SetOutline(Material outline)
    {
        this.outline = new Material(outline);
    }

    public Material GetOutline()
    {
        return outline;
    }

    public void SetNear(bool state)
    {
        near = state;
    }

    public bool IsPlayerNear()
    {
        return near;
    }
}
