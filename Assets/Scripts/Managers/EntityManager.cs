using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private static bool _movementEnabled;

    // returns whether or not the players and entities should be able to move or interact with the environment
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

    // trigger a player death
    public static void PlayerDied()
    {
        LevelManager.TriggerPlayerDeath();
    }
}
