using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum AudioNames
    {
        GunShot,
        Baseball,
        Unload,
        Reload
    }
    //Audio
    public AudioClip[] AttackAudio;
    private AudioSource audiosrc;
    public void PlayAudio(AudioNames index)
    {
        if ((int)index >= AttackAudio.Length) return;
        if (audiosrc != null) audiosrc.PlayOneShot(AttackAudio[(int)index], 0.75f);
    }
    // Start is called before the first frame update
    void Start()
    {
        audiosrc = gameObject?.GetComponent<AudioSource>();
    }
}
