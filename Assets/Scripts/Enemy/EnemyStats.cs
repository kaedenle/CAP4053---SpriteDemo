using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EnemyStats
{
    // movement variables
    public float minimumDistance = 1.0F;

    // reaction times
    public float slowReactionTime = 0.75F;
    public float fastReactionTime = 0.25F;
    public float memory_time = 5.0F;

    public enum SurpriseReactionType
    {
        Slow, 
        Fast
    }

    public float GetSurpriseReactionTime(SurpriseReactionType type)
    {
        return type == SurpriseReactionType.Slow ? slowReactionTime : fastReactionTime;
    }
}
