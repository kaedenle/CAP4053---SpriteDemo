using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeSorting : MonoBehaviour
{
    public GameObject Object;
    public bool front;
    private Renderer Rend;
    private Renderer myRend;
    private void UpdateLayers()
    {
        int delta = front ? 1 : -1;
        if (Rend.sortingOrder < 0) delta *= -1;
        if (Object != null && Rend != null && myRend != null) myRend.sortingOrder = Rend.sortingOrder + delta;  
    }
    // Start is called before the first frame update
    void Start()
    {
        Rend = Object?.GetComponent<Renderer>();
        myRend = gameObject?.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLayers();
    }
}
