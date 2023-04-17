using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class FOV
{
    public float radius;
    [Range(0.0F, 360.0F)] public float angle;
}