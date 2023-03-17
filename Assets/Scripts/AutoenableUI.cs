using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoenableUI : MonoBehaviour
{
    void Awake()
    {
        TurnOn();
    }

    void TurnOn()
    {
        foreach(Transform t in GetComponentsInChildren<Transform>(true))
        {
            t.gameObject.SetActive(true);
        }
    }
}
