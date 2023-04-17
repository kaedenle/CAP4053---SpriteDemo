using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEffectorFromState : MonoBehaviour
{
    public string eventID;
    public bool targetState = true;
    public GameObject[] enables;
    public GameObject[] disables;

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

            foreach(GameObject obj in disables)
            {
                if(obj == null) continue;

                obj.SetActive(false);
            }
        }
    }
}
