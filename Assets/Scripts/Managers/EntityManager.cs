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

    //hitstop variables
    private bool waiting = false;
    private bool ResumeTime = false;
    public void StopTime(float duration)
    {
        if (waiting || Time.timeScale != 1)
            return;
        //change this if want to go slower rather than stop
        Time.timeScale = 0;
        StopCoroutine(Wait(duration));
        StartCoroutine(Wait(duration));
    }
    IEnumerator Wait(float amt)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(amt);
        //ResumeTime = true;
        Time.timeScale = 1f;
        waiting = false;
    }

    private void Update()
    {
        if (ResumeTime)
        {
            if (Time.timeScale < 1f)
            {
                float time = Time.deltaTime == 0 ? 0.1f : Time.deltaTime * 3;
                Time.timeScale += time;
            }
            else
            {
                Time.timeScale = 1f;
                ResumeTime = false;
            }
        }
    }
}
