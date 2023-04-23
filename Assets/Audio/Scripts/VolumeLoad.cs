using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VolumeLoad : MonoBehaviour
{
    public AudioSource Music;
    private static int phase;
    // Start is called before the first frame update
    void Start()
    {
        phase = GameData.GetInstance().GetLevel();
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
        else if (phase == 4)
        {
            Destroy(this.gameObject);
        }
        
    }
    private void Update() {
        if (!EntityManager.MovementEnabled())
        {
            Music.Pause();
        }
        else
            Music.Play();
    }

    
}
