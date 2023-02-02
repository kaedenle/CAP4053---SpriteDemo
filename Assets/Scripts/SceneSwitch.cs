using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public int nextScene;

    void OnTriggerEnter2D(Collider2D other)
    {
        SceneManager.LoadScene("SampleScene");
    }

}
