using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A class for storing the beginning of attack data
//sounds and the text files
[System.Serializable]
public class AttackClass
{
    public TextAsset moveData;
    public AudioClip[] HitSFX;
}
