using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ItemClass
{
    public GameObject item;
    //figure out how the item is gonna drop
    public float percentChance;
    public bool usePlayerHealth;
}
