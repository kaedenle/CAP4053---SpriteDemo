using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Data/ExponentialProbability")]
public class ExponentialProbability : RespawnRate
{
    // Equation:
    // (initial) * (factor) ^ (x - guarenteed)
    // intention is to only respawn 1 enemy at a time in the house scenes
    [Range(0, 1)] public float initialProb;
    [Range(0, 1)] public float baseProb;
    public int guarenteedEnemies;

    public override int RespawnNumber(int totalSpawned)
    {
        if(totalSpawned < guarenteedEnemies) return 1;

        if(Random.Range(0, 0.99F) < GetProbability(totalSpawned) ) return 1;
        else return 0; 
    }

    public virtual double GetProbability(int totalSpawned)
    {
        return initialProb * System.Math.Pow(baseProb, totalSpawned - guarenteedEnemies);
    }
}
