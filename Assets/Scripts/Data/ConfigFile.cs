using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Data/ConfigFile")]
public class ConfigFile : ScriptableObject
{
    public ScenesData defaultSceneData;
    public ScenesData[] scenesData;
    public float easyModeSpeed, hardModeSpeed;

    public ScenesData GetScenesData(ScenesManager.AllScenes scene)
    {
        foreach(ScenesData sd in scenesData)
            if(sd.ContainsScene(scene))
                return sd;

        if( GeneralFunctions.IsDebug() ) Debug.LogWarning("using default scenes data for this scene");
        return defaultSceneData;
    }

    public float GetSpeed(float modifier = 1f)
    {
        float speed = GameData.GetInstance().GetDifficulty() == GameData.Difficulty.Easy ? easyModeSpeed : hardModeSpeed;
        return speed * modifier;
    }

    public float GetPlayerSpeed()
    {
        if(GameData.GetInstance().GetDifficulty() == GameData.Difficulty.Easy) return GetSpeed() + 1; // gives slight advantage to player
        else return GetSpeed();
    }

    public int GetRespawnNumber(int totalEnemies)
    {
        return GetScenesData(ScenesManager.GetCurrentScene()).GetRespawnNumber(totalEnemies);
    }
}
