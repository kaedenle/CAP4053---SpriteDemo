using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    [SerializeField] ScenesManager.AllScenes _nextScene;

    void OnTriggerEnter2D(Collider2D other)
    {
        // should probably throw an exception here if the scene info is not valid
        Debug.Log("switching to scene " + _nextScene.ToString());
        ScenesManager.LoadScene(_nextScene);
    }

}
