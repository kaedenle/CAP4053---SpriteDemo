using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildLevelManager : SubLevelManager
{
    private static bool inMirror = false;

    new void Awake()
    {
        startScene = ScenesManager.AllScenes.ChildLivingRoom;
        SetInstance(this);

        base.Awake();
    }

    public override void TriggerReset()
    {
        ResetVariables();
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