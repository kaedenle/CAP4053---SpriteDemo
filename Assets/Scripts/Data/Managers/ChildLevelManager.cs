using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildLevelManager : SubLevelManager
{
    private static bool inMirror = false;

    new void Start()
    {
        startScene = ScenesManager.AllScenes.ChildLivingRoom;
        SetInstance(this);

        base.Start();
    }

    public override void TriggerReset()
    {
        ResetVariables();
    }

    public static void ResetVariables()
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