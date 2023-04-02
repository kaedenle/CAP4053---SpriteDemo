using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleLevelManager : LevelManager
{
    private static bool maze_status = false;
    private static InventoryManager.AllItems[] specials = {
        InventoryManager.AllItems.CastleChestKey,
        InventoryManager.AllItems.CastleDagger
    };

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

    public static bool ObtainedPrereqs(int special)
    {
        for(int i = 0; i < special; i++)
            if(!InventoryManager.PickedUp(specials[i]))
                return false;
        return true;
    }
}
