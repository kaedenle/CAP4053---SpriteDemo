using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // _interact specifies whether the player can interact with the enviornment
    // _attack holds whether the player can attack
    // _move holds whether the player can move
    // _ui holds whether the user can interact w/ any UI components
    private static bool _interact, _attack, _move, _ui, _equip, _swap;

    public enum Keys
    {
        Interact,
        Hit1,
        Hit2,
        Pause,
        Equip,
        Swap,
        Continue
    }

    // the actual key bindings the player uses
    private static Dictionary<Keys, KeyCode> keycodes;

    // Default Key Bindings (in case the player wants to revert to default)
    private static Dictionary<Keys, KeyCode> defaultKeyCodes = new Dictionary<Keys, KeyCode>
    {
        {Keys.Interact, KeyCode.F},
        {Keys.Hit1, KeyCode.Mouse0},
        {Keys.Hit2, KeyCode.Mouse1},
        {Keys.Pause, KeyCode.Escape},
        {Keys.Equip, KeyCode.LeftShift},
        {Keys.Swap, KeyCode.Space},
        {Keys.Continue, KeyCode.Space}
    };

    void Awake()
    {
        if(keycodes == null)
        {
            keycodes = defaultKeyCodes;
        }
    }

    public static float GetAxis(string axisName)
    {
        if(!_move) return 0;

        return Input.GetAxis(axisName);
    }

    public static KeyCode GetKeyCode(Keys key)
    {
        return keycodes.GetValueOrDefault(key, KeyCode.None);
    }

    private static bool Pressed(Keys key)
    {
        return Input.GetKeyDown(GetKeyCode(key));
    }

    public static bool ContinueKeyPressed()
    {
        return _ui && (Pressed(Keys.Continue) || Pressed(Keys.Interact));
    }

    // tells you whether a specific key was pressed
    public static bool InteractKeyDown()
    {
        return _interact && (Pressed(Keys.Interact) || Input.GetKeyDown(KeyCode.E));
    }

    public static bool PauseKeyDown()
    {
        return Pressed(Keys.Pause);
    }

    public static bool Hit1KeyDown()
    {
        return _attack && Pressed(Keys.Hit1);
    }

    public static bool Hit2KeyDown()
    {
        return _attack && Pressed(Keys.Hit2);
    }

    public static bool SwapKeyDown()
    {
        return _swap && Pressed(Keys.Swap);
    }

    public static bool EquipKeyDown()
    {
        return _equip && Pressed(Keys.Equip);
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

    public static bool CanSwap()
    {
        return _swap;
    }

    public static bool CanEquip()
    {
        return _equip;
    }

    void Update()
    {
        _interact = EntityManager.IsEnvironmentInteractable();
        _attack = EntityManager.AttacktEnabled();
        _move = EntityManager.MovementEnabled();
        _ui = EntityManager.IsUIInteractable();
        _equip = EntityManager.EquipEnabled();
        _swap = EntityManager.SwapEnabled();
    }
}

    // public static bool KeyPressed(Keys key)
    // {
    //     KeyCode real_key = KeyCode.None;

    //     switch(key)
    //     {
    //         case Keys.Interact:
    //             if(_interact)
    //                 real_key = KeyCode.E;
    //             break;
            
    //         case Keys.Hit1:
    //             if(_attack)
    //                 real_key = KeyCode.Mouse0;
    //             break;
            
    //         case Keys.Hit2:
    //             if(_attack)
    //                 real_key = KeyCode.Mouse1;
    //             break;

    //         case Keys.Pause:
    //             real_key = KeyCode.Escape;
    //             break;

    //         case Keys.Equip:
    //             if(_equip)
    //                 real_key = KeyCode.LeftShift;
    //             break;

    //         case Keys.Swap:
    //             if(_swap)
    //                 real_key = KeyCode.Space;
    //             break;
            
    //         /*case Keys.Continue:
    //             if(_ui)
    //                 real_key = KeyCode.Space;
    //             break;*/
    //     }

    //     // default case (just return default)
    //     return Input.GetKeyDown(real_key);
    // }