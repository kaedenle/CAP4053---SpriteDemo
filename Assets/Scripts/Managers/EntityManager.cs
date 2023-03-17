using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private static bool _movementEnabled = true, _uiInteractable = true, _envInteractable = true, _attackEnabled = true, _swapEnabled = true, _equipEnabled;
    private static bool _pause = false;
    private static EntityManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public static bool IsPaused()
    {
        return _pause;
    }

    public static void Pause()
    {
        _pause = true;
        DisableMovement();
        DisableAttack();
        DisableEnvironmentInteractable();
        DisableUIInteractable();
        DisableSwap();
        DisableEquip();
        Time.timeScale = 0;
    }

    public static void Unpause()
    {
        _pause = false;
        EnableMovement();
        EnableAttack();
        EnableEnvironmentInteractable();
        EnableUIInteractable();
        EnableEquip();
        EnableSwap();
        Time.timeScale = 1;
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

    public static void EnableSwap()
    {
        _swapEnabled = true;
    }
    public static void DisableSwap()
    {
        _swapEnabled = false;
    }
    public static bool SwapEnabled()
    {
        return _swapEnabled;
    }
    public static void EnableEquip()
    {
        _equipEnabled = true;
    }
    
    public static void DisableEquip()
    {
        _swapEnabled = false;
    }
    public static bool EquipEnabled()
    {
        return _swapEnabled;
    }
    public static bool MovementEnabled()
    {
        return _movementEnabled;
    }

    // turn movement on
    public static void EnableMovement()
    {
        _movementEnabled = true;
    }
    
    // turn movement off
    public static void DisableMovement()
    {
        _movementEnabled = false;
    }

    public static bool AttacktEnabled()
    {
        return _attackEnabled;
    }

    // turn attack on
    public static void EnableAttack()
    {
        _attackEnabled = true;
    }
    
    // turn attack off
    public static void DisableAttack()
    {
        _attackEnabled = false;
    }

    // trigger a player death
    public static void PlayerDied()
    {
        LevelManager.TriggerPlayerDeath();
    }

    public static bool IsUIInteractable()
    {
        return _uiInteractable;
    }

    public static void EnableUIInteractable()
    {
        _uiInteractable = true;
    }

    public static void DisableUIInteractable()
    {
        _uiInteractable = false;
    }

    public static void EnableEnvironmentInteractable()
    {
        _envInteractable = true;
    }

    public static void DisableEnvironmentInteractable()
    {
        _envInteractable = false;
    }

    public static bool IsEnvironmentInteractable()
    {
        return _envInteractable;
    }

    
}
