using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineLoader : MonoBehaviour
{
    public Material outline;

    void Awake()
    {
        // set all objects with the Interactive Script to have the outline material
        foreach (OutlineObject obj in FindObjectsOfType(typeof(OutlineObject)))
        {
            obj.SetOutline(outline);
        }
    }
}
