using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindDoorBehavior : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        LevelManager.TriggerEnd();
    }
}
