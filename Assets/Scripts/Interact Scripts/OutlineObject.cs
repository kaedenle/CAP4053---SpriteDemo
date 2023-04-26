using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineObject : MonoBehaviour
{
    public OutlineParam outlineThickness;
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

        // automatically set offsets
        float temp_x = transform.localScale.x;
        float temp_y = transform.localScale.y;
        outlineMaterial.SetFloat("_Offset_x", 1); 
        outlineMaterial.SetFloat("_Offset_y", temp_x / temp_y); 

        // try to set outline thickness
        float thickness = GetThickness();
        outlineMaterial.SetFloat("_Outline_Thickness", thickness); 

        if(outlineMaterial != null) 
        {
            MakeOutlineObject();
        }
    }

    public void Update()
    {
        if(near && outlineRenderer != null)
        {
            outlineRenderer.sprite = sprite_renderer.sprite;

            // reset the ratios bc why not
            float temp_x = transform.localScale.x;
            float temp_y = transform.localScale.y;
            outlineMaterial.SetFloat("_Offset_x", 1); 
            outlineMaterial.SetFloat("_Offset_y", temp_x / temp_y); 
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            near = true;
            OverrideThickness();
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

    public void PrintSizes()
    {
        Vector2 sprite_size = GetComponent<SpriteRenderer>().sprite.rect.size;
        Vector2 local_sprite_size = sprite_size / GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        Vector3 world_size = local_sprite_size;
        world_size.x *= transform.lossyScale.x;
        world_size.y *= transform.lossyScale.y;

        //convert to screen space size
        Vector3 screen_size = 0.5f * world_size / Camera.main.orthographicSize;
        screen_size.y *= Camera.main.aspect;

        //size in pixels
        Vector3 in_pixels = new Vector3(screen_size.x * Camera.main.pixelWidth, screen_size.y * Camera.main.pixelHeight, 0) * 0.5f;

        // Debug.Log(string.Format("Sprite Size: {3}, Local Sprite Size: {4}, World size: {0}, Screen size: {1}, Pixel size: {2}",world_size,screen_size,in_pixels,sprite_size, local_sprite_size));
        float relative_x = sprite_size.x / screen_size.x;
    }

    public float GetThickness()
    {
        if(outlineThickness != null) return outlineThickness.outlineThickness;
        if((sprite_renderer = GetComponent<SpriteRenderer>()) == null) return 0;
        // sprite size
        Vector2 sprite_size = GetComponent<SpriteRenderer>().sprite.rect.size;
        // local size
        Vector2 local_sprite_size = sprite_size / GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        // world size
        Vector3 world_size = local_sprite_size;
        world_size.x *= transform.lossyScale.x;
        world_size.y *= transform.lossyScale.y;
        // screen size
        Vector3 screen_size = 0.5f * world_size / Camera.main.orthographicSize;

        // relative sizing
        float relative_x = sprite_size.x / screen_size.x;

        return (float) (0.00678712 * System.Math.Pow(relative_x, 0.898786) + 0.714275);
    }

    public void OverrideThickness()
    {
        if(outlineThickness != null)
        {
            if(outlineThickness.outlineThickness == 20) Debug.Log("outline thickness was set to 20");
            outlineMaterial.SetFloat("_Outline_Thickness", outlineThickness.outlineThickness); 
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

        // outlineRenderer.sortingLayerName = "Outline";
        outlineRenderer.sortingLayerName = sprite_renderer.sortingLayerName;
        outlineRenderer.sortingOrder = sprite_renderer.sortingOrder;
    }
}
