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
            Debug.Log("destroying this");
            Destroy(this.gameObject);
        }
    }
}
