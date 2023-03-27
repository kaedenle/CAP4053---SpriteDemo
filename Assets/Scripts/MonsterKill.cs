using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterKill : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collider)
    {
        LevelManager.ReloadScene();
    }
}
