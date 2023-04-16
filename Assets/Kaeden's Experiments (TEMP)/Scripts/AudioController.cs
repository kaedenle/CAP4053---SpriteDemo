using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private IDictionary<string, AudioManager> audiosrc;
    public void PlayAudioVerbose(AnimationEvent events)
    {
        float use = 0.5f;
        if (VolumeManager.instance != null) use = VolumeManager.instance.soundEffectValue;
        if (!audiosrc.ContainsKey(events.stringParameter)) return;
        if (events.intParameter >= audiosrc[events.stringParameter].AttackAudio.Length) return;
        if (audiosrc[events.stringParameter].AttackAudio != null) audiosrc[events.stringParameter].myaudiosrc.PlayOneShot(audiosrc[events.stringParameter].AttackAudio[events.intParameter], use);
    }
    // Start is called before the first frame update
    void Start()
    {
        AudioManager[] AM = gameObject?.GetComponentsInChildren<AudioManager>();
        audiosrc = new Dictionary<string, AudioManager>();
        foreach (AudioManager audio in AM)
            audiosrc.Add(audio.gameObject.name, audio);
    }
}
