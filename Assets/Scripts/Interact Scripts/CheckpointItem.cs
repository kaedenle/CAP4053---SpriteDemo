using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointItem : Item
{
    protected override void ActivateBehaviors()
    {
        base.ActivateBehaviors();
        GameData.GetInstance().SaveCurrentData();
    }
}
