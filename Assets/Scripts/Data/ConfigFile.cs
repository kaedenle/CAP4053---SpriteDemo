using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Data/ConfigFile")]
public class ConfigFile : ScriptableObject
{
    public ScenesData defaultSceneData;
    public ScenesData[] scenesData;
    public float baseSpeed = 12f;

    public float fovStoryMod = 0.6f;

    private Dictionary<Attribute, float> mod = new Dictionary<Attribute, float>
    {
        {Attribute.CapoFOVRadius, 0.8f},
        {Attribute.CapoFOVAngle, 0.85f},
        {Attribute.CapoWalkSpeed, 0.75f},
        {Attribute.EntitySpeed, 0.75f},
        {Attribute.PlayerSpeed, 0.9f}
    };

    public ScenesData GetScenesData(ScenesManager.AllScenes scene)
    {
        foreach(ScenesData sd in scenesData)
            if(sd.ContainsScene(scene))
                return sd;

        if( GeneralFunctions.IsDebug() ) Debug.LogWarning("using default scenes data for this scene");
        return defaultSceneData;
    }

    public float GetModifier(Attribute attribute)
    {
        if(GetDifficulty() == GameData.Difficulty.Hard) return 1;
        return mod[attribute];
    }

    public float GetSpeed(float modifier = 1f)
    {
        return baseSpeed * GetModifier(Attribute.EntitySpeed);
    }

    public float GetPlayerSpeed()
    {
        return baseSpeed * GetModifier(Attribute.PlayerSpeed);
    }

    public int GetRespawnNumber(int totalEnemies)
    {
        return GetScenesData(ScenesManager.GetCurrentScene()).GetRespawnNumber(totalEnemies);
    }

    public int GetMaxActive(ScenesManager.AllScenes scene)
    {
        return GetScenesData(scene).maxActiveEnemies;
    }

    public int GetMaxActive()
    {
        return GetMaxActive(ScenesManager.GetCurrentScene());
    }

    public GameData.Difficulty GetDifficulty()
    {
        return GameData.GetInstance().GetDifficulty();
    }

    public enum Attribute
    {
        CapoFOVRadius,
        CapoFOVAngle,
        CapoWalkSpeed,
        EntitySpeed,
        PlayerSpeed
    }
}
