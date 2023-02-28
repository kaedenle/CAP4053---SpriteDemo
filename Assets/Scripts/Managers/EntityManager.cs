using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private static bool _movementEnabled;

    public static bool MovementEnabled()
    {
        return _movementEnabled;
    }

    public static void EnableMovement()
    {
        _movementEnabled = true;
    }

    public static void DisableMovement()
    {
        _movementEnabled = false;
    }

    public static void PlayerDied()
    {
        LevelManager.TriggerPlayerDeath();
    }
}
