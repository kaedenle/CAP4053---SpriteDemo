using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static float default_delay = 0.7F;

    private static LevelManager Instance;
    private static ScenesManager.AllScenes _startScene;
    private static bool _levelEnding = false;
    private static bool _playerDied = false;
    private static GameObject player;
    private static Vector3 spawnPosition;
    private static Dictionary<string, bool> objectState;

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if(objectState == null)
        {
            objectState = new Dictionary<string, bool>();
        }
    }

    public void Start()
    {
        EntityManager.Pause();
        // ResetVariables();
        EntityManager.WaitThenUnpause(default_delay);
    }

    protected LevelManager setInstance(LevelManager obj, ScenesManager.AllScenes start)
    {
        Instance = obj;
        _startScene = start;
        return this;
    }

    public static void ResetVariables()
    {
        _playerDied = false;
        _levelEnding = false;
        objectState = new Dictionary<string, bool>();
    }

    static void ResetAllVariables()
    {
        ResetVariables();
        InventoryManager.ResetVariables();

        if (Instance != null)
            Instance.TriggerReset();
    }

    public static void TriggerEnd()
    {
        // EntityManager.Pause();
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
        EntityManager.Pause(); // reset from player death
        if (Instance != null)
            ScenesManager.LoadScene(_startScene);
    }

    public virtual void TriggerReset() { } // used abstractly; kinda sketchy

    public static void ReloadScene()
    {
        SetRespawn();
        ScenesManager.ReloadScene();
    }

    public static void SetRespawn()
    {
        spawnPosition = player.transform.position;
    }

    public static Vector3 GetRespawnPosition()
    {
        return spawnPosition;
    }

    public static void ToggleInteractiveState(string id)
    {
        if(id == null) return;

        if(!objectState.ContainsKey(id))
            objectState.Add(id, false);

        objectState[id] = !objectState[id];
    }

    public static bool GetInteractiveState(string id)
    {
        if(id == null) return false;
        
        return objectState.GetValueOrDefault(id, false);
    }
}
