using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    // array of the starting mind scenes
    private static ScenesManager.AllScenes[] mindSceneStarts =
    {
        ScenesManager.AllScenes.MobsterRoadDemo,
        ScenesManager.AllScenes.ChildLivingRoom
    };

    // array of starting demo mind scenes
    private static ScenesManager.AllScenes[] demoSceneStarts =
    {
        ScenesManager.AllScenes.MobsterRoadDemo
    };

    private static int currentPhase = 0;

    // awake is called before start
    void Awake()
    {
        Debug.Log("called Awake() in HubManager");
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("called Start() in HubManager, current phase is " + currentPhase);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void LoadNextMind()
    {
        currentPhase++;
        ScenesManager.LoadScene(mindSceneStarts[currentPhase - 1]);
    }
}
