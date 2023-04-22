using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Data/ConfigFile")]
public class ConfigFile : ScriptableObject
{
    public ScenesData defaultSceneData;
    public ScenesData[] scenesData;
}
