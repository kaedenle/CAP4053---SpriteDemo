using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// default RespawnRate (defaults to none)
[CreateAssetMenu(menuName = "ScriptableObject/Data/RespawnRate")]
public class RespawnRate : ScriptableObject
{
    public virtual int RespawnNumber(int totalSpawned) { return 0; }
}
