using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MovementStats
{
    public float speed = 6.0F;
    public float relativeSpeed = 1f;
    public float minimumDistance = 1.25F;
    public float attackChargeTime = 0.0F;
    [SerializeField] public FOV smallFOV;
    [SerializeField] public FOV mediumFOV;
    [SerializeField] public FOV largeFOV;

    public enum FOVType
    {
        Small,
        Medium,
        Large
    }

    public FOV GetFOV(FOVType type)
    {
        if(type == FOVType.Small) return smallFOV;
        else if(type == FOVType.Medium) return mediumFOV;
        else if(type == FOVType.Large) return largeFOV;

        Debug.LogError("tried to access an invalid FOV type");
        return null;
    }

    public float GetRadius(FOVType type)
    {
        return GetFOV(type).radius;
    }

    public float GetAngle(FOVType type)
    {
        return GetFOV(type).angle;
    }

    public float GetSpeed()
    {
        return GameData.GetConfig().GetSpeed(relativeSpeed);
    }
}
