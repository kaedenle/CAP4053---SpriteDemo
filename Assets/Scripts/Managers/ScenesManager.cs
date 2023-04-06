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

    private static bool firstSceneDebug = true;
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
        ChildForest,
        ChildUnderground,
        ChildChildRoom,
        Boss_Arena,
        Boss_HallwayLeft,
        Boss_HallwayDown,
        Boss_HallwayRight,
        Boss_HallwayUp,
        Boss_LeverLeft,
        Boss_LeverDown,
        Boss_LeverRight,
        Boss_BossRoom,
        CastleArena,
        CastleMaze,
        TutorialSkyBox
    }

    // loads a scene
    public static void LoadScene(AllScenes scene)
    {
        _prevScene = _currentScene;
        _currentScene = scene;
        UIManager.EndScene();
        Instance.StartCoroutine(LoadSceneAfterDelay((int)scene));
    }

    public static void ReloadScene()
    {
        LoadScene((AllScenes) (SceneManager.GetActiveScene().buildIndex));
    }

    static IEnumerator LoadSceneAfterDelay(int build_idx)
    {
        yield return new WaitForSecondsRealtime(nextSceneDelay);
        SceneManager.LoadScene( build_idx );
        UIManager.EnableHealthUI();
    }

    // loads a scene based on demo boolean
    public static void LoadSceneChoice(AllScenes full, AllScenes demo)
    {
        if (_demo) 
            LoadScene(demo);

        else
            LoadScene(full);
    }

    public static AllScenes GetPreviousScene()
    {
        return _prevScene;
    }

        public static AllScenes GetCurrentScene()
    {
        return _currentScene;
    }

    public void Awake()
    {
        _currentScene = (AllScenes) SceneManager.GetActiveScene().buildIndex;

        // if you've inialized the game outside of the menu
        if(firstSceneDebug && _currentScene != AllScenes.Menu)
        {
            GameState temp = new GameState(0, 0);

            if(_currentScene == AllScenes.CentralHub)
            {
                temp  = new GameState(0, 0);
            }

            else if((int)_currentScene <= (int) AllScenes.MobsterAlleyDemo)
            {
                temp = new GameState(1, 1);
            }

            else if((int)_currentScene <= (int) AllScenes.ChildChildRoom)
            {
                temp = new GameState(3, 1);
            }

            else if((int) _currentScene <= (int) AllScenes.Boss_BossRoom)
            {
                temp = new GameState(4, 1);
            }

            else if((int) _currentScene <= (int) AllScenes.CastleMaze)
            {
                temp = new GameState(2, 1);
            }

            else if((int) _currentScene <= (int) AllScenes.TutorialSkyBox)
            {
                temp = new GameState(0, 1);
            }
            
            temp.Save();
        }

        Instance = this;
        firstSceneDebug = false;
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
