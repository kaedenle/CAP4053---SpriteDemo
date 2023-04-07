using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeExitTeleports : StartBoxController
{
    public bool mazeExitState;

    new void Awake()
    {
        if(CastleLevelManager.GetMazeStatus() == mazeExitState)
            base.Awake();
    }
}
