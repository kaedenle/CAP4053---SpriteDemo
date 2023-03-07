using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static bool _levelEnding = false;
    private static LevelManager Instance;
    private static ScenesManager.AllScenes _startScene;
    private static bool _playerDied = false;

    void Start()
    {
        ResetVariables();
    }

    protected void setInstance(LevelManager obj, ScenesManager.AllScenes start)
    {
        Instance = obj;
        _startScene = start;
    }

    static void ResetVariables()
    {
        _playerDied = false;
        _levelEnding = false;
    }

    static void ResetAllVariables()
    {
        ResetVariables();

        if (Instance != null)
            Instance.TriggerReset();
    }

    public static void TriggerEnd()
    {
        ResetAllVariables();
        _levelEnding = true;
    }

    public static bool IsEndOfLevel()
    {
        return _levelEnding;
    }

    public static bool PauseEnabled()
    {
        // current behavior: can't pause during cutscene or during player death
        return !IsEndOfLevel() && !PlayerDead(); 
    }

    public static void TriggerPlayerDeath()
    {
        _playerDied = true;
        EntityManager.Pause();
    }

    public static bool PlayerDead()
    {
        return _playerDied;
    }

    public static void RestartLevel()
    {   
        ResetAllVariables();
        EntityManager.Unpause(); // reset from player death
        if (Instance != null)
            ScenesManager.LoadScene(_startScene);
    }

    public virtual void TriggerReset() { } // used abstractly; kinda sketchy
}
