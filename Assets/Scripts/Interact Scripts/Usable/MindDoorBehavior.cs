using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindDoorBehavior : Interactive
{
    new void Awake()
    {
        base.Awake();
    }

    new void Start()
    {
        base.Start();
    }

    protected override void ActivateBehaviors()
    {
        base.ActivateBehaviors();
        LevelManager.TriggerEnd();
    }
}
