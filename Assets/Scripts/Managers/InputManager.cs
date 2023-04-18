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
        Continue,
        Up,
        Down,
        Left,
        Right
    }

    // the actual key bindings the player uses
    private static Dictionary<Keys, KeyPair> keycodes;

    // Default Key Bindings (in case the player wants to revert to default)
    private static Dictionary<Keys, KeyPair> defaultKeyCodes = new Dictionary<Keys, KeyPair>
    {
        {Keys.Interact, new KeyPair(KeyCode.F, KeyCode.E)},
        {Keys.Hit1, new KeyPair(KeyCode.Mouse0, KeyCode.None)},
        {Keys.Hit2, new KeyPair(KeyCode.Mouse1, KeyCode.None)},
        {Keys.Pause, new KeyPair(KeyCode.Escape, KeyCode.None)},
        {Keys.Equip, new KeyPair(KeyCode.LeftShift, KeyCode.None)},
        {Keys.Swap, new KeyPair(KeyCode.Space, KeyCode.None)},
        {Keys.Continue, new KeyPair(KeyCode.Space, KeyCode.None)},
        {Keys.Up, new KeyPair(KeyCode.W, KeyCode.UpArrow)},
        {Keys.Down, new KeyPair(KeyCode.S, KeyCode.DownArrow)},
        {Keys.Left, new KeyPair(KeyCode.A, KeyCode.LeftArrow)},
        {Keys.Right, new KeyPair(KeyCode.D, KeyCode.RightArrow)}
    };

    void Awake()
    {
        if(keycodes == null)
        {
            SetKeyCodes();
        }
    }

    void SetKeyCodes()
    {
        if(!KeyCodesExist())
            RevertToDefault();
        else   
            LoadKeyCodes();
    }

    public static void RevertToDefault()
    {
        keycodes = new Dictionary<Keys, KeyPair>(defaultKeyCodes);
        SaveKeyCodes();
    }

    public static void SaveKeyCodes()
    {
        foreach (KeyValuePair<Keys, KeyPair> entry in keycodes)
        {
            string prefString = entry.Key.ToString() + "Key";
            int primary = (int) entry.Value.GetPrimary();
            int secondary = (int) entry.Value.GetSecondary();

            Debug.Log("saving key " + prefString + " to keys " + ((KeyCode)primary).ToString() + " and " + ((KeyCode)secondary).ToString());
            PlayerPrefs.SetInt(prefString + "1", primary);
            PlayerPrefs.SetInt(prefString + "2", secondary);
        }
    }

    public static void LoadKeyCodes()
    {
        keycodes = new Dictionary<Keys, KeyPair>(defaultKeyCodes);
        Dictionary<Keys, KeyPair> loadedKeyCodes = new Dictionary<Keys, KeyPair>();

        if(GeneralFunctions.IsDebug()) Debug.Log("================ Input Keys ===============");
        foreach(KeyValuePair<Keys, KeyPair> entry in keycodes)
        {
            string prefString = entry.Key.ToString() + "Key";
            KeyCode primary = (KeyCode) PlayerPrefs.GetInt(prefString + "1");
            KeyCode secondary = (KeyCode) PlayerPrefs.GetInt(prefString + "2");

            if(GeneralFunctions.IsDebug()) Debug.Log("loaded key " + prefString + " to keys " + ((KeyCode)primary).ToString() + " and " + ((KeyCode)secondary).ToString());

            loadedKeyCodes[entry.Key] = new KeyPair(primary, secondary);
        }
        if(GeneralFunctions.IsDebug()) Debug.Log("=========================================");

        keycodes = loadedKeyCodes;
    }

    public static bool KeyCodesExist()
    {
        // doing the safest check
        foreach (KeyValuePair<Keys, KeyPair> entry in defaultKeyCodes)
        {
            string prefString = entry.Key.ToString() + "Key";

            if(!PlayerPrefs.HasKey(prefString + "1") || !PlayerPrefs.HasKey(prefString + "2"))
                return false;
        }

        return true;
    }

    public static float GetAxis(string axisName)
    {
        if(!_move) return 0;

        return Input.GetAxis(axisName);
    }

    public static KeyPair GetKeyCode(Keys key)
    {
        return keycodes.GetValueOrDefault(key, new KeyPair(KeyCode.None, KeyCode.None));
    }

    private static bool GetKeyDown(KeyCode code)
    {
        // TODO: deal with exceptions
        

        return Input.GetKeyDown(code);
    }

    private static bool Pressed(Keys key)
    {
        KeyPair pair = GetKeyCode(key);
        return GetKeyDown(pair.GetPrimary()) || GetKeyDown(pair.GetSecondary());
    }

    public static bool ContinueKeyPressed()
    {
        return _ui && (Pressed(Keys.Continue) || Pressed(Keys.Interact));
    }

    // tells you whether a specific key was pressed
    public static bool InteractKeyDown()
    {
        return _interact && (Pressed(Keys.Interact));
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

    public static KeyPair GetBindings(Keys key)
    {
        if(!keycodes.ContainsKey(key)) return new KeyPair(KeyCode.None, KeyCode.None);
        return keycodes[key];
    }

    public static Dictionary<Keys, KeyPair> GetBindings()
    {
        return keycodes;
    }

    public static void SaveNewBindings(Dictionary<Keys, KeyPair> bind)
    {
        keycodes = new Dictionary<Keys, KeyPair>(bind);
        SaveKeyCodes();
    }
}

public class KeyPair
{
    public KeyCode primary, secondary;

    public KeyPair(KeyCode pr, KeyCode se)
    {
        primary = pr;
        secondary = se;
    }

    public KeyCode GetPrimary()
    {
        return primary;
    }

    public KeyCode GetSecondary()
    {
        return secondary;
    }

    public bool UsesCode(KeyCode code)
    {
        return primary == code || secondary == code;
    }

    public bool RemoveCode(KeyCode code)
    {
        if(!UsesCode(code)) return false;

        if(code == primary)
        {
            primary = secondary;
        }

        secondary = KeyCode.None;
        return true;
    }

    public bool AddCode(KeyCode code)
    {
        if(UsesCode(code)) return false;

        if(secondary != KeyCode.None)
        {
            primary = secondary;
        }

        secondary = code;
        return true;
    }

    public bool IsSingle()
    {
        return secondary == KeyCode.None;
    }
}
