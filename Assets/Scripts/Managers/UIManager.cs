using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TO DO: make this a singleton
public class UIManager : MonoBehaviour
{
    private static bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // toggle between paused and unpaused
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("pressed esc");
            if(paused)
            {
                Unpause();
            }

            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        paused = true;
        EntityManager.DisableMovement();
    }

    void Unpause()
    {
        paused = false;
        EntityManager.EnableMovement();
    }

    public static bool IsPaused()
    {
        return paused;
    }
}
