using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipCutscene : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && SceneManager.GetActiveScene().name == "StartGameCutscene")
            SceneManager.LoadScene("Central Hub");
        else if (Input.GetKeyDown(KeyCode.Space) && SceneManager.GetActiveScene().name == "Credits")
            SceneManager.LoadScene("Menu");
    }
}
