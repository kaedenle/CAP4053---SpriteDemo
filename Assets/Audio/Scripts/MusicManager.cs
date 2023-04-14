using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip musicClip;
    public static VolumeController instance;


    void Start()
    {
        VolumeController.instance.audioSource.clip = musicClip;
        VolumeController.instance.audioSource.Play();
    }
}
