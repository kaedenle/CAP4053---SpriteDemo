using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioPlayer
{
    public AudioClip audioFile;

    public void PlayAudio()
    {
        if(audioFile == null) return;

        AudioLevelManager.PlayAudio(audioFile);
    }
}
