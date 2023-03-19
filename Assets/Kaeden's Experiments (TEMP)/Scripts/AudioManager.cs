using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Audio
    public AudioClip[] AttackAudio;
    private AudioSource audiosrc;
    public void PlayAudio(int index)
    {
        if (index >= AttackAudio.Length) return;
        if (audiosrc != null) audiosrc.PlayOneShot(AttackAudio[index], 0.75f);
    }
    // Start is called before the first frame update
    void Start()
    {
        audiosrc = gameObject?.GetComponent<AudioSource>();
    }
}
