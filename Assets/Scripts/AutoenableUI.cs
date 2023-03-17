using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoenableUI : MonoBehaviour
{
    private UnityEngine.EventSystems.UIBehaviour[] allUI;
    void Awake()
    {
        allUI = gameObject.GetComponentsInChildren<UnityEngine.EventSystems.UIBehaviour>();
        TurnOn();
    }

    void TurnOn()
    {
        Debug.Log("turning UIs on...");
        foreach(UnityEngine.EventSystems.UIBehaviour ui in allUI) 
        { 
            ui.enabled = true; //turn them on. 
        } 

        foreach(Transform t in GetComponentsInChildren<Transform>(true))
        {
            t.gameObject.SetActive(true); // = true; //SetActive(true);
            Debug.Log("testing transform..." + t.ToString());
        }
    }
}
