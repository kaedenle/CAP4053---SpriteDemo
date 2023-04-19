using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsableEnvironment : MonoBehaviour
{
    public int ID;
    public bool collapsed = false;
    public void Knock()
    {
        collapsed = true;
        foreach (Transform child in transform)
        {
            if (child.gameObject.name != "Unfallen" && child.gameObject.name != "Fallen") return;
            if (child.gameObject.name == "Unfallen") child.gameObject.SetActive(false);
            else if (child.gameObject.name == "Fallen") child.gameObject.SetActive(true);
        }
    }
}
