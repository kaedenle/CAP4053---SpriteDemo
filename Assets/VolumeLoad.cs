using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float savedVolume = VolumeManager.instance.volumeValue;
        GetComponent<AudioSource>().volume = savedVolume;
    }
}
