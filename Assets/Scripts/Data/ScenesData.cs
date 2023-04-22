using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Data/ScenesData")]
public class ScenesData : ScriptableObject
{
    public ScenesManager.AllScenes scene;
    public int maxActiveEnemies;
    public int maxTotalEnemies;
}
