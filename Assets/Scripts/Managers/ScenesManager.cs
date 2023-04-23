using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;
    private static ScenesManager.AllScenes _currentScene = AllScenes.Menu, _prevScene = AllScenes.Menu; // need to update these defaults after setting up central room
    private static bool _demo = false;
    const float nextSceneDelay = 1.0F;

    private static bool firstSceneDebug = true;
    public static EventHandler ChangedScenes;

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
        TutorialSkyBox,
        StartCutScene,
        Hallway,
        Classroom,
        AntonioHome,
        TrainStation,
        DormRoom
    }

    // loads a scene
    public static void LoadScene(AllScenes scene)
    {
        _prevScene = _currentScene;
        _currentScene = scene;
        UIManager.EndScene();
        if (ChangedScenes != null) ChangedScenes(null, EventArgs.Empty);
        Instance.StartCoroutine(LoadSceneAfterDelay((int)scene));
    }

    public static void ReloadScene()
    {
        
        LoadScene((AllScenes) (SceneManager.GetActiveScene().buildIndex));
    }

    static IEnumerator LoadSceneAfterDelay(int build_idx)
    {
        EntityManager.Pause();
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
        Instance = this;
        _currentScene = (AllScenes) SceneManager.GetActiveScene().buildIndex;

        // if you've inialized the game outside of the menu
        if(firstSceneDebug && _currentScene != AllScenes.Menu)
        {
            GameData.Difficulty dif = GameData.GetInstance().HasLoadData() ? GameData.GetInstance().GetDifficulty() : GameData.Difficulty.Hard;
            
            GameData data = GameData.GetInstance();
            data.ResetData();
            
            data.SetDifficulty(dif); // default to previous difficulty settings

            if(_currentScene == AllScenes.CentralHub)
            {
                // do nothing, setting start level as 0 is fine
            }

            else if(sceneToLevel.ContainsKey(_currentScene))
            {
                data.SetLevel((int) sceneToLevel[_currentScene]);
            }
            
            data.SaveCurrentData();
        }

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

    /*
    ============= Debug Stuff =============
    */
    Dictionary<AllScenes, HubManager.PhaseTag> sceneToLevel = new Dictionary<AllScenes, HubManager.PhaseTag>
    {
        {AllScenes.MobsterRoadDemo, HubManager.PhaseTag.Mobster},
        {AllScenes.MobsterRestaurantDemo, HubManager.PhaseTag.Mobster},
        {AllScenes.MobsterKitchenDemo, HubManager.PhaseTag.Mobster},
        {AllScenes.MobsterAlleyDemo, HubManager.PhaseTag.Mobster},
        {AllScenes.ChildLivingRoom, HubManager.PhaseTag.Child},
        {AllScenes.ChildParentBedroom, HubManager.PhaseTag.Child},
        {AllScenes.ChildPlayroom, HubManager.PhaseTag.Child},
        {AllScenes.ChildForest, HubManager.PhaseTag.Child},
        {AllScenes.ChildUnderground, HubManager.PhaseTag.Child},
        {AllScenes.ChildChildRoom, HubManager.PhaseTag.Child},
        {AllScenes.Boss_Arena, HubManager.PhaseTag.Boss},
        {AllScenes.Boss_HallwayLeft, HubManager.PhaseTag.Boss},
        {AllScenes.Boss_HallwayDown, HubManager.PhaseTag.Boss},
        {AllScenes.Boss_HallwayRight, HubManager.PhaseTag.Boss},
        {AllScenes.Boss_HallwayUp, HubManager.PhaseTag.Boss},
        {AllScenes.Boss_LeverLeft, HubManager.PhaseTag.Boss},
        {AllScenes.Boss_LeverDown, HubManager.PhaseTag.Boss},
        {AllScenes.Boss_LeverRight, HubManager.PhaseTag.Boss},
        {AllScenes.Boss_BossRoom, HubManager.PhaseTag.Boss},
        {AllScenes.CastleArena, HubManager.PhaseTag.Castle},
        {AllScenes.CastleMaze, HubManager.PhaseTag.Castle},
        {AllScenes.TutorialSkyBox, HubManager.PhaseTag.Tutorial},
        {AllScenes.Hallway, HubManager.PhaseTag.Mobster},
        {AllScenes.Classroom, HubManager.PhaseTag.Mobster},
        {AllScenes.AntonioHome, HubManager.PhaseTag.Mobster},
        {AllScenes.TrainStation, HubManager.PhaseTag.Mobster},
        {AllScenes.DormRoom, HubManager.PhaseTag.Mobster},

    };
}
