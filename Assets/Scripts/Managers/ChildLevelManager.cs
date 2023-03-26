using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildLevelManager : LevelManager
{
    private static bool inMirror = false;

    // Start is called before the first frame update
    new void Start()
    {
        setInstance(this, ScenesManager.AllScenes.ChildLivingRoom);
        base.Start();
    }

    new public static void ResetVariables()
    {
        inMirror = false;
    }

    public static void ToggleMirrorWorld()
    {
        inMirror = !inMirror;
    }

    public static bool InMirror()
    {
        return inMirror;
    }
}