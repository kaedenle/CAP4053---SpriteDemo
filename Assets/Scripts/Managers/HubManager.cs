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

    //// array of starting demo mind scenes
    private static ScenesManager.AllScenes[] demoSceneStarts =
    {
        ScenesManager.AllScenes.MobsterRoadDemo
    };

    private static int currentPhase = 0;

    // awake is called before start
    void Awake()
    {
        Debug.Log("called Awake() in HubManager");
        currentPhase++;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("called Start() in HubManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadNextMind()
    {
        currentPhase++;
        ScenesManager.LoadScene(mindSceneStarts[currentPhase - 1]);
    }
}
