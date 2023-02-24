using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public static ScenesManager.AllScenes _startScene;

    public abstract void ResetVariables();

    public static void RestartLevel()
    {
        Instance.ResetVariables();
        ScenesManager.LoadScene(_startScene);
    }

    public static void PresetInstance(LevelManager manager)
    {
        Instance = manager;
    }
    
}
