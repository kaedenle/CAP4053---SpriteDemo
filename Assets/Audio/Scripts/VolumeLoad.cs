using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VolumeLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float savedMusicVolume;
        float savedSoundEffectVolume;

        if (gameObject.tag == "LevelTheme")
        {
            savedMusicVolume = VolumeManager.instance.volumeValue;
            GetComponent<AudioSource>().volume = savedMusicVolume;
        }
        else if (gameObject.tag == "SoundEffect")
        {
            savedSoundEffectVolume = VolumeManager.instance.soundEffectValue;
            GetComponent<AudioSource>().volume = savedSoundEffectVolume;
        }
        
    }

    
}
