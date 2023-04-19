using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindDoorBehavior : Interactive
{
    new void Awake()
    {
        base.Awake();
        // outlineThickness = 7.0F;
    }

    new void Start()
    {
        base.Start();
        // base.GetOutline().SetFloat("_Outline_Thickness", outlineThickness);
    }

    protected override void ActivateBehaviors()
    {
        base.ActivateBehaviors();
        LevelManager.TriggerEnd();
    }
}
