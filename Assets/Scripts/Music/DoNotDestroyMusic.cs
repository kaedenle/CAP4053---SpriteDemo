using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoNotDestroyMusic : MonoBehaviour
{
    AudioSource m_MyAudioSource;
    private int level;
    // Start is called before the first frame update
    private void Awake() 
    {   
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("VolumeManager");
        GameObject[] audioObj = GameObject.FindGameObjectsWithTag("LevelTheme"); 

        if (musicObj.Length > 1)
            Destroy(this.gameObject);
        if (audioObj.Length > 1)
            Destroy(this.gameObject);
        if(SceneManager.GetActiveScene().name == "Central Hub")
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
        level = GameData.GetInstance().GetLevel();
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Central Hub")
            Destroy(this.gameObject);
        if (ScenesManager.GetCurrentScene() == ScenesManager.AllScenes.Menu)
            Destroy(this.gameObject);
        if (SceneManager.GetActiveScene().name == "Child Room")
        {
            m_MyAudioSource.Stop();
        }
        if (SceneManager.GetActiveScene().name == "Boss Room")
            Destroy(this.gameObject);
        else if (SceneManager.GetActiveScene().name == "Living Room" && !m_MyAudioSource.isPlaying)
        {
            m_MyAudioSource.Play();
        }
    }
}
