using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineObject : MonoBehaviour
{
    public float outlineThickness = 0.5F;
    private Material defaultMaterial;
    private Material outlineMaterial;
    private SpriteRenderer sprite_renderer;

    private bool near = false;
    GameObject outline;
    SpriteRenderer outlineRenderer;

    public void Awake()
    {
        // get the objects for the outlines
        sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        defaultMaterial = sprite_renderer.material;
    }

    public void Start()
    {
        if(outlineMaterial == null)
        {
            OutlineLoader loader = FindObjectOfType<OutlineLoader>();

            if(loader != null)
                outlineMaterial = loader.GetOutline();
        }

        if(outlineMaterial != null) 
        {
            outlineMaterial.SetFloat("_Outline_Thickness", outlineThickness); 
            MakeOutlineObject();
        }
    }

    public void Update()
    {
        if(near && outlineRenderer != null)
        {
            outlineRenderer.sprite = sprite_renderer.sprite;
        }
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
        // sprite_renderer.material = outlineMaterial;
        if(outline != null)
            outline.SetActive(true);
    }

    public void DisableOutline()
    {
        // sprite_renderer.material = defaultMaterial;
        if(outline != null)
            outline.SetActive(false);
    }

    public void SetOutline(Material outlineMaterial)
    {
        this.outlineMaterial = new Material(outlineMaterial);
    }

    public Material GetOutline()
    {
        return outlineMaterial;
    }

    public void SetNear(bool state)
    {
        near = state;
    }

    public bool IsPlayerNear()
    {
        return near;
    }

    public void MakeOutlineObject()
    {
        // check for something went wrong
        if(outlineMaterial == null || sprite_renderer == null) return;
        
        // make a new object to put the outline
        outline = new GameObject("Outline", typeof(SpriteRenderer));

        // set default state to off
        outline.SetActive(false);

        // set the outline's parent to this object
        outline.transform.parent = this.gameObject.transform;

        // set transform states
        outline.transform.localPosition = new Vector3(0F, 0F, 0F);
        outline.transform.localScale = new Vector3(1F, 1F, 1F);
        outline.transform.rotation = this.gameObject.transform.rotation;   

        // set the outline's sprite renderer variables
        outlineRenderer = outline.GetComponent<SpriteRenderer>();

        outlineRenderer.sprite = sprite_renderer.sprite;
        outlineRenderer.flipX = sprite_renderer.flipX;
        outlineRenderer.flipY = sprite_renderer.flipY;
        outlineRenderer.size = sprite_renderer.size;
        outlineRenderer.spriteSortPoint = sprite_renderer.spriteSortPoint;
        outlineRenderer.adaptiveModeThreshold = sprite_renderer.adaptiveModeThreshold;
        outlineRenderer.drawMode = sprite_renderer.drawMode;
        outlineRenderer.maskInteraction = sprite_renderer.maskInteraction;


        // set outline material
        outlineRenderer.material = outlineMaterial;

        outlineRenderer.sortingLayerName = "Outline";
    }
}
