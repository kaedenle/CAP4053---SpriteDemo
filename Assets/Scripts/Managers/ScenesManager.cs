using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;
    private static ScenesManager.AllScenes _currentScene = AllScenes.Menu, _prevScene = AllScenes.Menu; // need to update these defaults after setting up central room
    private static bool _demo = false;
    private static float nextSceneDelay = 1.0F;
    // update this enum whenever you add (or remove) a Scene (must be in same order as in building settings)
    // edit these names at your own risk; so many things use these, so you'll have to track them all down if
    // you want to change these (adding more is fine, as long as you add to the end)
    public enum AllScenes
    {
        Menu = 0,
        CentralHub,
        MobsterRoadDemo,
        MobsterRestaurantDemo,
        MobsterKitchenDemo,
        MobsterAlleyDemo,
        ChildLivingRoom,
        ChildParentBedroom,
        ChildPlayroom,
        ChildForest
    }

    // loads a scene
    public static void LoadScene(AllScenes scene)
    {
        _prevScene = _currentScene;
        _currentScene = scene;
        UIManager.EndScene();
        Instance.StartCoroutine(LoadSceneAfterDelay((int)scene));
    }

    static IEnumerator LoadSceneAfterDelay(int build_idx)
    {
        yield return new WaitForSecondsRealtime(nextSceneDelay);
        SceneManager.LoadScene( build_idx );
    }

    // loads a scene based on demo boolean
    public static void LoadSceneChoice(AllScenes full, AllScenes demo)
    {
        if (_demo) 
            LoadScene(demo);

        else
            LoadScene(full);
    }

    static void Reset()
    {
        HubManager.ResetVariables();
        MobsterLevelManager.ResetVariables();
    }

    // loads the first scene (that's not the menu)
    public static void LoadNewGame()
    {
        Reset();
        // load the first scene here
        _demo = false;
        LoadScene(AllScenes.CentralHub);
    }

    public static void StartDemo()
    {
        Reset();
        _demo = true;
        // set default level manager
        LoadScene(AllScenes.MobsterRoadDemo);
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
