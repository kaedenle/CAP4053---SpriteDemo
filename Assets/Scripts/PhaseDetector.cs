using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseDetector : MonoBehaviour
{
    public HubManager.PhaseTag phase;

    void Start()
    {
        if(!HubManager.TagIsCurrent(phase))
            Destroy(gameObject);
    }
}
