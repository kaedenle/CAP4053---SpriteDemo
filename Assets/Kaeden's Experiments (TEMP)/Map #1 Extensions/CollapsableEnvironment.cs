using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsableEnvironment : MonoBehaviour
{
    public void Knock()
    {
        GameObject perform = transform.parent.gameObject;
        foreach(Transform child in perform.transform)
        {
            if (child.gameObject.name != "Unfallen" && child.gameObject.name != "Fallen") return;
            if (child.gameObject.name == "Unfallen") child.gameObject.SetActive(false);
            else if (child.gameObject.name == "Fallen") child.gameObject.SetActive(true);
        }
    }
}
