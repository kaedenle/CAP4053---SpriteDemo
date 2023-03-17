using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TO DO: make this a singleton
public class UIManager : MonoBehaviour
{
    private static bool paused = false;

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
