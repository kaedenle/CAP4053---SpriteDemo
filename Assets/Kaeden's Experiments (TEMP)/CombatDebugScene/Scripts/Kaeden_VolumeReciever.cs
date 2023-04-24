using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kaeden_VolumeReciever : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isBGM;
    public static float BGMVolume = 0.5f;
    public static float SFXVolume = 0.5f;
    private float PrevValue;
    private AudioSource myAud;
    public bool giveUpControl;
    void Awake()
    {
        myAud = GetComponent<AudioSource>();
        UpdateVolume();
        giveUpControl = false;
    }
    public void UpdateVolume()
    {
        if (giveUpControl) return;
        float use = isBGM ? BGMVolume : SFXVolume;
        if (myAud != null) myAud.volume = use;
        PrevValue = use;
    }
    public float GetVolume()
    {
        return isBGM ? BGMVolume : SFXVolume;
    }
    // Update is called once per frame
    void Update()
    {
        if ((isBGM && PrevValue != BGMVolume) || (!isBGM && PrevValue != SFXVolume)) UpdateVolume();
    }
}
