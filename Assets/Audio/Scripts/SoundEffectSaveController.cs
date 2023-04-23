using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundEffectSaveController : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private TMP_Text volumeTextUI = null;

    private void Start() 
    {
        LoadValues();
        VolumeManager.instance.soundEffectValue = 0.5f;
        volumeSlider.value = VolumeManager.instance.soundEffectValue;
    }

    public void VolumeSlider(float volume)
    {
        volumeTextUI.text = volume.ToString("0.0");
        VolumeManager.instance.soundEffectValue = volume;
        ApplyVolumeToAudioSources();
    }

    public void SaveVolumeButton()
    {
        float soundEffectValue = volumeSlider.value;
        PlayerPrefs.SetFloat("SoundEffectValue", soundEffectValue);
        LoadValues();
    }

    void LoadValues()
    {
        float soundEffectValue = PlayerPrefs.GetFloat("SoundEffectValue");
        volumeSlider.value = soundEffectValue;
        VolumeManager.instance.soundEffectValue = soundEffectValue;
        ApplyVolumeToAudioSources();
    }

    void ApplyVolumeToAudioSources() {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources) {
            if (audioSource.tag == "SoundEffect") {
                audioSource.volume = VolumeManager.instance.soundEffectValue;
            }
        }
    }
}
