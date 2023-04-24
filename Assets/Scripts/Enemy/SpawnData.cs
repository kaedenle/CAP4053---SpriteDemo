using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public Dictionary<string, List<Base>> activeEnemies  = new Dictionary<string, List<Base>>();
    public int[] killed = new int[100];

    public SpawnData() {}
    public SpawnData(SpawnData cpy) 
    {
        activeEnemies = new Dictionary<string, List<Base>>(cpy.activeEnemies);
        killed = cpy.killed.Clone() as int[];
    }

    public bool ContainsSpawner(string id)
    {
        return activeEnemies.ContainsKey(id);
    }

    public int GetKills(ScenesManager.AllScenes scene)
    {
        return killed[(int) scene];
    }

    public void StoreEnemies(List<Base> enemies, string key)
    {
        // Debug.Log("storing spawn key " + key);
        if(activeEnemies.ContainsKey(key)) activeEnemies[key] = enemies;
        else activeEnemies.Add(key, enemies);
    }

    public List<Base> LoadEnemies(string key)
    {
        // Debug.Log("loading spawn key " + key);
        return activeEnemies.GetValueOrDefault(key, null);
    }

    public void IncrementKills(ScenesManager.AllScenes scene)
    {
        killed[(int) scene] ++;
    }
}
