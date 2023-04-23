using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSFX : MonoBehaviour
{
    public string runSFX;
    private Animator anim;
    private  AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = RunSFXManager.GetSource();
        anim = GetComponent<Animator>();
        //audioSource = GetComponent<AudioSource>();
        //SoundEffectManager.PlayAudio(runSFX);
    }
    private void ActivateScript()
    {
        RunSFXManager.PlayAudio(runSFX);
    }
    private void StopPlay()
    {
        if(audioSource != null) audioSource.Stop();
    }
    // Update is called once per frame
    void Update()
    {
        if(!audioSource.isPlaying && anim.GetFloat("movement") != 0)
        {
            ActivateScript();
           
        }
        if (anim.GetFloat("movement") < 0.001) StopPlay();
    }
}
