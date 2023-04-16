using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EnemyStats
{
    // movement variables
    public float minimumDistance = 1.0F;
    public float speed = 6.0F;

    // reaction times
    public float surprise_reaction_time = 1.0F;
    public float memory_time = 1.0F;
}
