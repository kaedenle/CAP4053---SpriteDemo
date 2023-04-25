using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneSwitch : MonoBehaviour
{
    [SerializeField] public ScenesManager.AllScenes nextScene;
    public bool demoForks = false;
    [SerializeField] public ScenesManager.AllScenes nextDemoScene;

    private string playerTag = "Player";

    void OnTriggerEnter2D(Collider2D other)
    {
        // make sure the collider is the player
        if (other.gameObject.tag != playerTag) return;
        
        // sends the next scene or scenes to ScenesManager to load the next appropriate scene
        // if (demoForks) ScenesManager.LoadSceneChoice(nextScene, nextDemoScene);
        ScenesManager.LoadScene(nextScene);
    } // should probably throw an exception here if the scene info is not valid
}