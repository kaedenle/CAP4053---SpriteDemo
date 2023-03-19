using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    private Material defaultMaterial;
    private Material outline;
    private SpriteRenderer renderer;
    private bool near;

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

    void Awake()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        defaultMaterial = renderer.material;
        near = false;
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
}
