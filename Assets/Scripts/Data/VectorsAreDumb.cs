using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class VectorsAreDumb
{
    float x, y, z;
    public VectorsAreDumb(Vector3 vec)
    {
        if(vec == null) return;

        x = vec.x;
        y = vec.y;
        z = vec.z;
    }

    public Vector3 Get()
    {
        return new Vector3(x, y, z);
    }

    public override string ToString()
    {
        return "(" + x + ", " + y  + ", " + z + ")";
    }
}