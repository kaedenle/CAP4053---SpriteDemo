using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Kaeden_VolumeController : MonoBehaviour
{
    public float SFX {get; set;}
    public float BGM { get; set;}
    [System.Serializable]
    public struct VolumeUI
    {
        public Slider Slider;
        public TMP_Text text;
        public bool isBGM;
    }
    public VolumeUI[] VUI;
    // Start is called before the first frame update
    void Start()
    {

        SFX = Kaeden_VolumeReciever.SFXVolume;
        BGM = Kaeden_VolumeReciever.BGMVolume;
        LoadValues();
        if (VUI.Length > 1)
        {
            VUI[1].text.text = SFX.ToString("0.0");
            VUI[1].Slider.value = SFX;
        }
        if (VUI.Length > 0)
        {
            VUI[0].text.text = BGM.ToString("0.0");
            VUI[0].Slider.value = BGM;
        }
    }
    private void OnDisable()
    {
        SaveVolumeButton();
    }
    public void ChangeSFX(float value)
    {
        SFX = value;
    }
    public void ChangeBGM(float value)
    {
        BGM = value;
    }
    public void SaveVolumeButton()
    {
        float BGMValue = VUI.Length > 0 ? VUI[0].Slider.value : BGM;
        float SFXValue = VUI.Length > 1 ? VUI[1].Slider.value : SFX;
        PlayerPrefs.SetFloat("SFXValue", SFXValue);
        PlayerPrefs.SetFloat("BGMValue", BGMValue);
    }

    void LoadValues()
    {
        if (PlayerPrefs.HasKey("SFXValue")) SFX = PlayerPrefs.GetFloat("SFXValue");
        if (PlayerPrefs.HasKey("BGMValue")) BGM = PlayerPrefs.GetFloat("BGMValue");
    }
    private void Limit()
    {
        if (SFX > 1) SFX = 1;
        else if (SFX < 0) SFX = 0;
        if (BGM > 1) BGM = 1;
        else if (BGM < 0) BGM = 0;
    }
    // Update is called once per frame
    void Update()
    {
        Limit();
        if (SFX != Kaeden_VolumeReciever.SFXVolume)
        {
            Kaeden_VolumeReciever.SFXVolume = SFX;
            if (VUI.Length > 1) VUI[1].text.text = SFX.ToString("0.0");
        }
        if (BGM != Kaeden_VolumeReciever.BGMVolume)
        {
            Kaeden_VolumeReciever.BGMVolume = BGM;
            if(VUI.Length > 0) VUI[0].text.text = BGM.ToString("0.0");
        }
    }
}
