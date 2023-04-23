using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLevelManager : MonoBehaviour
{
    public static AudioSource source;

    public static void PlayAudio(AudioClip audioFile)
    {
        if(source == null) return;
        
        source.PlayOneShot(audioFile);
    }
}
