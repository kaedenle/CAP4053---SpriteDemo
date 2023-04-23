using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Data/ConfigFile")]
public class ConfigFile : ScriptableObject
{
    public ScenesData defaultSceneData;
    public ScenesData[] scenesData;
    public float baseSpeed;

    public ScenesData GetScenesData(ScenesManager.AllScenes scene)
    {
        foreach(ScenesData sd in scenesData)
            if(sd.ContainsScene(scene))
                return sd;
        return defaultSceneData;
    }

    public float GetSpeed(float modifier = 1f)
    {
        return baseSpeed * modifier;
    }

    public int GetRespawnNumber(int totalEnemies)
    {
        return GetScenesData(ScenesManager.GetCurrentScene()).GetRespawnNumber(totalEnemies);
    }
}
