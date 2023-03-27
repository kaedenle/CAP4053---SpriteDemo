using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnEvent : MonoBehaviour
{
    public string eventID;
    public bool targetState = true;
    public GameObject[] enables;

    void Update()
    {
        bool activated = LevelManager.GetInteractiveState(eventID);

        if(activated == targetState)
        {
            foreach(GameObject obj in enables)
            {
                if(obj == null) continue;

                obj.SetActive(false);
            }

            Destroy(gameObject);
        }
    }
}
