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
        Unpause();
    }

    // Update is called once per frame
    void Update()
    {
        // toggle between paused and unpaused
        if(InputManager.PauseKeyDown() && LevelManager.PauseEnabled())
        {
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
        EntityManager.Pause();
    }

    void Unpause()
    {
        paused = false;
        EntityManager.Unpause();
    }

    public static bool IsPaused()
    {
        return paused;
    }   
}
