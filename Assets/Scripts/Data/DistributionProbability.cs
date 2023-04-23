using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Data/DistributionProbability")]
public class DistributionProbability : ExponentialProbability
{
    // Equation:
    // (initial) * (factor) ^ (x - guarenteed)
    // intention is to only respawn 1 enemy at a time in the house scenes
    public int minRespawn, maxRespawn;

    public override int RespawnNumber(int totalSpawned)
    {
        double total = 0;
        List<double> probs = new List<double>();

        // get raw %
        for(int i = minRespawn; i <= maxRespawn; i++)
        {
            double p = GetProbability(totalSpawned + minRespawn);
            probs.Add(p);
            total += p;
        }

        // normalize
        for(int i = 0; i < probs.Count; i++)
            probs[i] /= total;
        
        double gen = Random.Range(0, 0.999999999999F);
        int idx = 0;

        while(gen > probs[idx])
        {
            gen -= probs[idx];
            idx++;
        }

        return minRespawn + idx;
    }
}
