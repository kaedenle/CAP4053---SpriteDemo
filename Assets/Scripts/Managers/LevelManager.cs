using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static bool _levelEnding = false;

    void Start()
    {
        _levelEnding = false; // reset
    }

    public static void TriggerEnd()
    {
        _levelEnding = true;
    }

    public static bool IsEndOfLevel()
    {
        return _levelEnding;
    }

    //public static LevelManager Instance;
    //public static ScenesManager.AllScenes _startScene;

    //public abstract void ResetVariables();

    //public static void RestartLevel()
    //{
    //    Instance.ResetVariables();
    //    ScenesManager.LoadScene(_startScene);
    //}

    //public static void PresetInstance(LevelManager manager)
    //{
    //    Instance = manager;
    //}

}
