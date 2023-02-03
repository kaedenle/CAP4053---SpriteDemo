using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public bool useSceneID;
    public int sceneID;

    public bool useSceneName;
    public string sceneName;

    void OnTriggerEnter2D(Collider2D other)
    {
        // should probably throw an exception here if the scene info is not valid

        if(useSceneID)
            SceneManager.LoadScene(sceneID);

        else
            SceneManager.LoadScene(sceneName);
    }

}
