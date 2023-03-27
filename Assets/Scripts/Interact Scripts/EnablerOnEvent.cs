using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablerOnEvent : MonoBehaviour
{
    public string eventID;
    public bool targetState = true;
    public GameObject[] enables;

    void Start()
    {
        bool activated = LevelManager.GetInteractiveState(eventID);

        if(activated == targetState)
        {
            foreach(GameObject obj in enables)
            {
                if(obj == null) continue;

                obj.SetActive(true);
            }
        }

        Destroy(gameObject);
    }
}
