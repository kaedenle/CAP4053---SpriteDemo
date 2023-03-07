using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static bool _interact, _attack, _move, _ui;

    public enum Keys
    {
        Interact = KeyCode.E,
        Hit1 = KeyCode.Mouse0,
        Hit2 = KeyCode.Mouse1,
        Pause = KeyCode.Escape,

    }

    public static float GetAxis(string axisName)
    {
        if(!_move) return 0;

        return Input.GetAxis(axisName);
    }

    public static bool KeyPressed(Keys key)
    {
        KeyCode real_key = KeyCode.None;

        switch(key)
        {
            case Keys.Interact:
                if(_interact)
                    real_key = KeyCode.E;
                break;
            
            case Keys.Hit1:
                if(_attack)
                    real_key = KeyCode.Mouse0;
                break;
            
            case Keys.Hit2:
                if(_attack)
                    real_key = KeyCode.Mouse1;
                break;

            case Keys.Pause:
                real_key = KeyCode.Escape;
                break;
        }

        return Input.GetKeyDown(real_key);
    }

    // tells you whether a specific key was pressed
    public static bool InteractKeyDown()
    {
        return _interact && KeyPressed(Keys.Interact);
    }

    public static bool PauseKeyDown()
    {
        return KeyPressed(Keys.Pause);
    }

    public static bool Hit1KeyDown()
    {
        return _attack && KeyPressed(Keys.Hit1);
    }

    public static bool Hit2KeyDown()
    {
        return _attack && KeyPressed(Keys.Hit2);
    }

    // get the booleans here if necessary (they will also be in EntityManager)
    public static bool CanAttack()
    {
        return _attack;
    }

    public static bool CanMove()
    {
        return _move;
    }

    public static bool UIInteractable()
    {
        return _ui;
    }

    public static bool EnvironmentInteractable()
    {
        return _interact;
    }


    void Update()
    {
        _interact = EntityManager.IsEnvironmentInteractable();
        _attack = EntityManager.AttacktEnabled();
        _move = EntityManager.MovementEnabled();
        _ui = EntityManager.IsUIInteractable();
    }
}
