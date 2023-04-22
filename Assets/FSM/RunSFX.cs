using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSFX : MonoBehaviour
{
    public string runSFX;
    private  AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = RunSFXManager.GetSource();
        //audioSource = GetComponent<AudioSource>();
        //SoundEffectManager.PlayAudio(runSFX);
    }

    // Update is called once per frame
    void Update()
    {
        if(!audioSource.isPlaying)
        {
            RunSFXManager.PlayAudio(runSFX);
           
        }
         this.enabled = false;
    }
}
