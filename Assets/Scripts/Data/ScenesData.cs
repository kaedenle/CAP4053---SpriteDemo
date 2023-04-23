using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Data/ScenesData")]
public class ScenesData : ScriptableObject
{
    public ScenesManager.AllScenes[] scenes;
    public int maxActiveEnemies;
    public int maxTotalEnemies;

    public RespawnRate respawnBehavior;

    public int GetRespawnNumber(int totalSpawned)
    {
        if(respawnBehavior == null) respawnBehavior = new RespawnRate();

        return System.Math.Min( respawnBehavior.RespawnNumber(totalSpawned), maxTotalEnemies - totalSpawned );
    }
    
    public bool ContainsScene(ScenesManager.AllScenes scene)
    {
        foreach(ScenesManager.AllScenes s in scenes)
            if(s == scene)
                return true;
        return false;
    }
}
