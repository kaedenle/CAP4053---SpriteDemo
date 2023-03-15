using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DebugSceneManager : MonoBehaviour
{
    private GameObject player;
    public GameObject book;
    private void SceneInputs()
    {
        //reset level
        if (!EntityManager.IsPaused() && Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //kill yourself
        if (Input.GetKeyDown(KeyCode.K))
            player.GetComponent<HealthTracker>().healthSystem.Damage(10000);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            book.SetActive(true);
            book.GetComponent<BookUI>().ToggleBook();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
            book.GetComponent<BookUI>().SkipAnim();
    }

    void Awake()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        SceneInputs();
    }
}
