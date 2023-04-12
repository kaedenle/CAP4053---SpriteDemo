using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EntityManager : MonoBehaviour
{
    private static bool _pause = false;
    private static int numPauses = 0;
    private static EntityManager Instance;

    public enum AllStates
    {
        _movementEnabled,
        _uiInteractable,
        _envInteractable,
        _attackEnabled,
        _swapEnabled,
        _equipEnabled,
    }

    private static bool[] states; 

    void Awake()
    {
        Instance = this;

        if(states == null)
        {
            states = new bool[System.Enum.GetValues(typeof(AllStates)).Length];
            Array.Fill(states, true);
        }
    }

    public static bool IsPaused()
    {
        return _pause;
    }

    public static void SetPause()
    {
        _pause = true;

        if(numPauses <= 0)
            Time.timeScale = 0;

        numPauses ++;
    }

    public static void SetUnpause()
    {
        numPauses--;

        if(numPauses <= 0)
        {
            if(numPauses < 0)
            {
                numPauses = 0;
                Debug.LogError("pause count was set to less than zero");
            }
            _pause = false;
            Time.timeScale = 1;
        }
    }

    public static int PauseAndMask()
    {
        int mask = 0;
        SetPause();

        for(int i = 0, d = 1; i < states.Length; i++, d <<= 1)
        {
            if(states[i])
            {
                mask |= d;
                DisableState((AllStates) i);
            }
        }

        return mask;
    }

    public static void UnpauseWithMask(int mask)
    {
        for(int i = 0, d = 1; i < states.Length; i++, d <<= 1)
        {
            if((mask & d) != 0)
            {
                EnableState((AllStates) i);
            }
        }

        SetUnpause();
    }
    public static void Pause()
    {
        SetPause();
        Array.Fill(states, false);
    }

    public static void SceneStartPause()
    {
        Debug.Log("SceneStartPause()");
        
        _pause = true;
        numPauses = 1;
        Time.timeScale = 0;
        Array.Fill(states, false);
    }

    public static void Unpause()
    {
        SetUnpause();
        Array.Fill(states, true);
    }

    public static void DialoguePause()
    {
        SetPause();

        // disable specific states
        DisableMovement();
        DisableAttack();
        DisableEnvironmentInteractable();
        DisableSwap();
        DisableEquip();
    }

    public static void FalsePause()
    {
        DisableMovement();
        DisableAttack();
        DisableSwap();
        DisableEquip();
    }

    public static void SetHubStates()
    {
        DisableSwap();
        DisableEquip();
    }

    public static void WaitThenUnpause(float delay)
    {
        Instance.StartCoroutine(Instance.DoDelayedUnpause(delay));
    }

    IEnumerator DoDelayedUnpause(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Unpause();
    }

    // trigger a player death
    public static void PlayerDied()
    {
        LevelManager.TriggerPlayerDeath();
    }

    public static void EnableState(AllStates state)
    {
        states[(int) state] = true;
    }

    public static void DisableState(AllStates state)
    {
        states[(int) state] = false;
    }

    public static bool GetState(AllStates state)
    {
        return states[(int) state];
    }

    public static void EnableSwap()
    {
        EnableState(AllStates._swapEnabled);
    }

    public static void DisableSwap()
    {
        DisableState(AllStates._swapEnabled);
    }

    public static bool SwapEnabled()
    {
        return GetState(AllStates._swapEnabled);
    }

    public static void EnableEquip()
    {
        EnableState(AllStates._equipEnabled);
    }
    
    public static void DisableEquip()
    {
        DisableState(AllStates._equipEnabled);
    }

    public static bool EquipEnabled()
    {
        return GetState(AllStates._equipEnabled);
    }

    // turn movement on
    public static void EnableMovement()
    {
        EnableState(AllStates._movementEnabled);
    }
    
    // turn movement off
    public static void DisableMovement()
    {
        DisableState(AllStates._movementEnabled);
    }

    public static bool MovementEnabled()
    {
        return GetState(AllStates._movementEnabled);
    }

    // turn attack on
    public static void EnableAttack()
    {
        EnableState(AllStates._attackEnabled);
    }
    
    // turn attack off
    public static void DisableAttack()
    {
        DisableState(AllStates._attackEnabled);
    }

    public static bool AttacktEnabled()
    {
        return GetState(AllStates._attackEnabled);
    }

    public static void EnableUIInteractable()
    {
        EnableState(AllStates._uiInteractable);
    }

    public static void DisableUIInteractable()
    {
        DisableState(AllStates._uiInteractable);
    }

    public static bool IsUIInteractable()
    {
        return GetState(AllStates._uiInteractable);
    }

    public static void EnableEnvironmentInteractable()
    {
        EnableState(AllStates._envInteractable);
    }

    public static void DisableEnvironmentInteractable()
    {
       DisableState(AllStates._envInteractable);
    }

    public static bool IsEnvironmentInteractable()
    {
        return GetState(AllStates._envInteractable);
    }

    public static bool isPaused()
    {
        return _pause;
    }
}
