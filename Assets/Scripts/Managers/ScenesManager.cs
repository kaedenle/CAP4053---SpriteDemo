using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;

    // update this enum whenever you add (or remove) a Scene (must be in same order as in building settings)
    // edit these names at your own risk; so many things use these, so you'll have to track them all down if
    // you want to change these (adding more is fine, as long as you add to the end)
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
