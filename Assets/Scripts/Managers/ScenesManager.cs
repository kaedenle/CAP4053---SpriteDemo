using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;
    private static ScenesManager.AllScenes _currentScene = AllScenes.SampleScene, _prevScene = AllScenes.SampleScene; // need to update these defaults after setting up central room
    private static bool _demo = false;
    // update this enum whenever you add (or remove) a Scene (must be in same order as in building settings)
    // edit these names at your own risk; so many things use these, so you'll have to track them all down if
    // you want to change these (adding more is fine, as long as you add to the end)
    public enum AllScenes
    {
        CentralHub = 0,
        MobsterRoadDemo,
        MobsterRestaurantDemo,
        MobsterKitchenDemo,
        MobsterAlleyDemo,
        ChildLivingRoom,
        ChildKitchen,
        ChildPlayroom,
        ChildForest,
        SampleScene
    }

    // loads a scene
    public static void LoadScene(AllScenes scene)
    {
        _prevScene = _currentScene;
        _currentScene = scene;
        SceneManager.LoadScene( (int) scene);
    }
    
    // loads the first scene (that's not the menu)
    public static void LoadNewGame()
    {
        // load the first scene here
        // TEMP FIRST SCENE
        LoadScene(AllScenes.CentralHub);
    }

    public static AllScenes GetPreviousScene()
    {
        return _prevScene;
    }

    public void Awake()
    {
        Instance = this;
    }

    public static void setDemo()
    {
        _demo = true;
    }

    public static bool isDemo()
    {
        return _demo;
    }
}
