using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorEnabler : MonoBehaviour
{
    public bool insideMirror;
    public GameObject toEnable;
    void Awake()
    {
        if(insideMirror == ChildLevelManager.InMirror())
        {
            if(toEnable != null)
                toEnable.SetActive(true);
        }

        Destroy(gameObject);
    }
}
