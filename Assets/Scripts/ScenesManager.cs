using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;

    // update this enum whenever you add (or remove) a Scene (must be in same order as in building settings)
    public enum AllScenes
    {
        SampleScene = 0,
        MobsterRoadDemo,
        MobsterRestaurantDemo,
        MobsterAlleyDemo
    }

    // loads a scene
    public static void LoadScene(AllScenes scene)
    {
        SceneManager.LoadScene( (int) scene);
    }
    
    // loads the first scene (that's not the menu)
    public static void LoadNewGame()
    {
        // load the first scene here
        // TEMP FIRST SCENE
        LoadScene(AllScenes.MobsterRoadDemo);
    }

    public void Awake()
    {
        Instance = this;
    }
}
