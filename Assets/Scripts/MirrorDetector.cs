using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorDetector : MonoBehaviour
{
    public bool insideMirror;
    void Awake()
    {
        if(insideMirror ^ ChildLevelManager.InMirror())
        {
            Destroy(this.gameObject);
        }
    }
}
