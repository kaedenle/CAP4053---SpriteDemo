using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kaeden_RunningSFX : MonoBehaviour
{
    private AudioSource mysrc;
    private Animator anim;
    private void Start()
    {
        mysrc = GetComponent<AudioSource>();
        anim = GetComponentInParent<Animator>();
    }
    void Update()
    {
        if (mysrc != null && !mysrc.isPlaying && anim != null && anim.GetFloat("movement") > 0)
        {
            mysrc.Play();
        }
        else if (mysrc.isPlaying && anim != null && anim.GetFloat("movement") < 0.001f) mysrc.Stop();
    }
}
