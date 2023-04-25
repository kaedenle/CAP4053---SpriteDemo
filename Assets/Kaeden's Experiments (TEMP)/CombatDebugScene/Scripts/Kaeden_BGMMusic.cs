using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Kaeden_BGMMusic : MonoBehaviour
{
    [System.Serializable]
    public struct MusicPlayer{
        public AudioClip music;
        public ScenesManager.AllScenes scene;
    }

    public MusicPlayer[] mp;
    private IDictionary<ScenesManager.AllScenes, AudioClip> musicMap;
    private AudioSource src;
    public static GameObject instance = null;
    private AudioClip playing;
    private IEnumerator fading;
    private ScenesManager.AllScenes currentScene;
    private Kaeden_VolumeReciever reciever;
    public float fade;
    public float duration;
    void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null) instance = gameObject;
        else if (instance != gameObject) Destroy(gameObject);
    }
    private void OnDisable()
    {
        ScenesManager.ChangedScenes -= OnNewSceneLoaded;
    }
    private void Start()
    {

        musicMap = new Dictionary<ScenesManager.AllScenes, AudioClip>();
        reciever = GetComponent<Kaeden_VolumeReciever>();
        for (int i = 0; i < mp.Length; i++)
        {
            if (!musicMap.ContainsKey(mp[i].scene)) musicMap.Add(mp[i].scene, mp[i].music);
        }
        src = GetComponent<AudioSource>();
        PlayMusic();
        ScenesManager.ChangedScenes += OnNewSceneLoaded;
    }
    public IEnumerator StartFade(float duration, float targetVolume)
    {
        if (src == null) yield break;
        float currentTime = 0;
        float start = src.volume;
        float decreaseTime = fade;
        while (currentTime < duration)
        {
            currentTime += decreaseTime;
            src.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        fading = null;
        PlayMusic();
        yield break;
    }
    public void PlayMusic()
    {
        currentScene = ScenesManager.GetCurrentScene();
        if (musicMap.ContainsKey(currentScene))
        {
            src.clip = musicMap[currentScene];
            playing = src.clip;
            src.Stop();
            src.volume = reciever != null ? reciever.GetVolume() : 0.5f;
            if (playing != null)
            {
                src.PlayOneShot(playing);
                src.PlayScheduled(AudioSettings.dspTime + playing.length);
            }
        }
    }
    public void OnNewSceneLoaded(object o, ScenesManager.AllScenes e)
    {
          if (!musicMap.ContainsKey(e)) return;
        //if clip going into not the same, don't do anything
        if (playing != musicMap[e])
        {
            if (fading != null) StopCoroutine(fading);
            fading = StartFade(duration, 0f);
            StartCoroutine(fading);
        }
        currentScene = ScenesManager.GetCurrentScene();
    }
    public void StopPlay()
    {
        src.Stop();
    }
    public void Update()
    {
        if (currentScene != ScenesManager.GetCurrentScene()) PlayMusic(); 
    }
}
