using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    private Material defaultMaterial;
    private Material outline;
    private SpriteRenderer renderer;

    public void OnTriggerEnter2D()
    {
        EnableOutline();
    }

    public void OnTriggerExit2D()
    {
        DisableOutline();
    }

    void Awake()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        defaultMaterial = renderer.material;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
