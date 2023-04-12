using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointDoor : Door
{
    protected override void ActivateBehaviors()
    {
        GameData.GetInstance().SaveAfterSceneChange();
        base.ActivateBehaviors();
    }
}
