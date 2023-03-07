using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private static bool _movementEnabled = true, _uiInteractable = true, _envInteractable = true, _attackEnabled = true;
    private static bool _pause = false;

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
        Time.timeScale = 0;
    }

    public static void Unpause()
    {
        _pause = false;
        EnableMovement();
        EnableAttack();
        EnableEnvironmentInteractable();
        EnableUIInteractable();
        Time.timeScale = 1;
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
