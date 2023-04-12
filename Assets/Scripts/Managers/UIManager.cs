using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TO DO: make this a singleton
public class UIManager : MonoBehaviour
{
    private static bool paused = false;
    private static bool isSwitching = false;
    private static bool renderHealthBars = true;
    public GameObject MenuUI;
    public static Dictionary<string, int> interactive_index;
    private static int pause_mask = 0;
    private static GameObject HealthBarUI;

    void Awake()
    {
        paused = false;
        
        GameObject UIBar = GameObject.Find("-- UI --");
        HealthBarUI = GameObject.Find("/-- UI --/UI Canvas");
        if (HealthBarUI == null) HealthBarUI = Instantiate(Resources.Load("Prefabs/UI Canvas")) as GameObject;
        if(UIBar != null) HealthBarUI.transform.SetParent(UIBar.transform);

        MenuUI = GameObject.Find("/-- UI --/Menu Canvas/Book UI");
        isSwitching = false;

        if(interactive_index == null)
        {
            interactive_index = new Dictionary<string, int>();
        }

        EnableHealthUI();
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

            if (MenuUI != null)
            {
                MenuUI.SetActive(true);
                MenuUI.GetComponent<BookUI>().ToggleBook();
            }
        }
    }

    public static void ResetManager()
    {
        pause_mask = 0;
        interactive_index = new Dictionary<string, int>();
    }

    static void Pause()
    {
        paused = true;
        pause_mask = EntityManager.PauseAndMask();
    }

    void Unpause()
    {
        paused = false;
        EntityManager.UnpauseWithMask(pause_mask);
    }

    public static bool IsPaused()
    {
        return paused;
    }

    public static void EndScene()
    {
        isSwitching = true;
    }

    public static bool SceneSwitching()
    {
        return isSwitching;
    }
    
    public static int GetInteractiveIndex(string id)
    {
        return interactive_index.GetValueOrDefault(id, 0);
    }

    public static void SetInteractiveIndex(string id, int value)
    {
        interactive_index[id] = value;
    }

    public static Dictionary<string, int> GetStates()
    {
        return interactive_index;
    }

    public static void SetStates(Dictionary<string, int> ii)
    {
        interactive_index = ii;
    }

    //for seeing if health bars should be rendered
    public static void DisableHealthUI()
    {
        foreach (Transform child in HealthBarUI.transform)
            child.gameObject.SetActive(false);
        renderHealthBars = false;
    }
    public static void EnableHealthUI()
    {
        foreach (Transform child in HealthBarUI.transform)
            child.gameObject.SetActive(true);
        renderHealthBars = true;
    }
    public static bool getHealthUI()
    {
        return renderHealthBars;
    }
}
