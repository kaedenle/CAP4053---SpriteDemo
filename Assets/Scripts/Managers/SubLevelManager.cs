using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubLevelManager : MonoBehaviour
{
    protected static SubLevelManager Instance;
    protected static ScenesManager.AllScenes startScene = ScenesManager.AllScenes.Menu;

    protected void Start()
    {
        if(Instance != null)
        {
            LevelManager.SetLevel(Instance, startScene);
        }
    }

    protected void SetInstance(SubLevelManager obj)
    {
        Instance = obj;
    }

    virtual public void TriggerReset()
    {
        // nothing by default
    }
}
