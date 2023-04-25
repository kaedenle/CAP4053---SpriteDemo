using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAddCamToCanvas : MonoBehaviour
{
    void Awake()
    {
        gameObject.GetComponent<Canvas>().worldCamera = GeneralFunctions.GetMainCamera();
    }
}
