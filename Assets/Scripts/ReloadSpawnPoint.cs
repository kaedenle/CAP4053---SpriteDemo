using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadSpawnPoint : MonoBehaviour
{
    public GameObject player;

    void Awake()
    {
        if(ScenesManager.GetPreviousScene() == ScenesManager.GetCurrentScene())
        {
            player.transform.position = ChildLevelManager.GetRespawnPosition();
        }
    }

}
