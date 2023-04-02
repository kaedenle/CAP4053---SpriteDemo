using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleLevelManager : LevelManager
{
    private static bool maze_status = false;

    new void Start()
    {
        setInstance(this, ScenesManager.AllScenes.CastleArena);
        base.Start();
    }

    new public static void ResetVariables()
    {
        maze_status = true;
    }

    public static void SetMazeStatus(bool status)
    {
        maze_status = status;
    }
}
