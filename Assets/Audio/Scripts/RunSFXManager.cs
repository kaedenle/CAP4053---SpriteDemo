using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSFXManager : MonoBehaviour
{
    static AudioClip[] sfxList;
    public static AudioSource source;
    // Start is called before the first frame update
    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);

    }
    void Start()
    {
        source = GetComponent<AudioSource>();
       // Debug.Log("helloooooo");
        sfxList = Resources.LoadAll<AudioClip>("SoundEffects");
       // Debug.Log(sfxList.Length);
        for(int i= 0; i < sfxList.Length; ++i)
        {
            //Debug.Log("hello");
            //Debug.Log(sfxList[i].ToString());
        }
        //DontDestroyOnLoad(this.gameObject);
    }

    public static void PlayAudio(string name)
    {
        // don't play the audio if there is no specified audio
        if(name.Equals("")) return;

        //Debug.Log("Playing audio or somethin");
        //Debug.Log(name);
        for(int i = 0; i < sfxList.Length; ++i)
        {
            //Debug.Log(sfxList[i].ToString());
            if(sfxList[i].ToString().Equals(name + " (UnityEngine.AudioClip)"))
            {
                //Debug.Log("Playing " + name);
                source.PlayOneShot(sfxList[i]);
                break;
            }
        }
    }
    public static AudioSource GetSource()
    {
        return source;
    }
    public static AudioClip getAudioClip(string name)
    {
        for(int i = 0; i < sfxList.Length; ++i)
        {
         //   Debug.Log(sfxList[i].ToString());
            if(sfxList[i].ToString().Equals(name + " (UnityEngine.AudioClip)"))
            {
                Debug.Log("Playing " + name);
                return sfxList[i];
            }
        }
        return null;
    }

}
