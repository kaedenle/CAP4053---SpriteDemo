using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TO DO: make this a singleton
public class UIManager : MonoBehaviour
{
    private static bool paused = false;
    private static bool isSwitching = false;
    public GameObject MenuUI;

    void Awake()
    {
        isSwitching = false;
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
                UIPause();
            }
            if (MenuUI != null)
            {
                MenuUI.SetActive(true);
                MenuUI.GetComponent<BookUI>().ToggleBook();
            }
        }
    }

    static void UIPause()
    {
        paused = true;
        Pause();
    }

    static void Pause()
    {
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

    public static void EndScene()
    {
        isSwitching = true;
        EntityManager.Pause();
    }

    public static bool SceneSwitching()
    {
        return isSwitching;
    }
}
