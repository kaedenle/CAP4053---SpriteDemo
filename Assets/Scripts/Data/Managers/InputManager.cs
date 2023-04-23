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

    // All input keys for our game
    public const int NUMBER_OF_KEYS = 11;
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
        {Keys.Hit1, new KeyPair(KeyCode.Mouse0, KeyCode.Keypad1)},
        {Keys.Hit2, new KeyPair(KeyCode.Mouse1, KeyCode.Keypad2)},
        {Keys.Pause, new KeyPair(KeyCode.Escape, KeyCode.None)},
        {Keys.Equip, new KeyPair(KeyCode.LeftShift, KeyCode.None)},
        {Keys.Swap, new KeyPair(KeyCode.Space, KeyCode.Keypad3)},
        {Keys.Continue, new KeyPair(KeyCode.Space, KeyCode.None)},
        {Keys.Up, new KeyPair(KeyCode.W, KeyCode.UpArrow)},
        {Keys.Down, new KeyPair(KeyCode.S, KeyCode.DownArrow)},
        {Keys.Left, new KeyPair(KeyCode.A, KeyCode.LeftArrow)},
        {Keys.Right, new KeyPair(KeyCode.D, KeyCode.RightArrow)}
    };

    // makes sure there are keys setup
    void Awake()
    {
        if(keycodes == null)
        {
            SetKeyCodes();
        }
    }

    // sets the keycodes at the start of the game
    void SetKeyCodes()
    {
        if(!KeyCodesExist())
            RevertToDefault();
        else   
            LoadKeyCodes();
    }

    // resets the keys bindings to the default bindings
    public static void RevertToDefault()
    {
        keycodes = new Dictionary<Keys, KeyPair>(defaultKeyCodes);
        SaveKeyCodes();
    }

    // saves the current key bindings
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

    // loads the saved key bindings
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

    // checks if the key bindings are already saved
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

    // TODO: replace this method with something that checks the bindings
    //       alternatively, lock the Up, Down, Left, Right bindings
    public static float GetAxis(string axisName)
    {
        if(!_move) return 0;

        return Input.GetAxis(axisName);
    }

    // returns the keycode pair assciated with a Key
    public static KeyPair GetKeyCode(Keys key)
    {
        return keycodes.GetValueOrDefault(key, new KeyPair(KeyCode.None, KeyCode.None));
    }

    // returns whether a (true) keycode is pressed
    private static bool GetKeyDown(KeyCode code)
    {
        // code = GetTrueKey(code); 

        return Input.GetKeyDown(code);
    }

    private static bool Pressed(KeyCode keycode)
    {
        keycode = GetTrueKey(keycode);
        foreach(KeyCode code in GetKeysToCheck(keycode))
            if(GetKeyDown(code))
            {
                return true;
            }
        return false;
    }

    private static bool Pressed(Keys key)
    {
        KeyPair pair = GetKeyCode(key);
        return Pressed(pair.GetPrimary()) || Pressed(pair.GetSecondary());
    }
    //-----------------------------------------------------------------------
    private static bool Hold(KeyCode keycode)
    {
        foreach (KeyCode code in GetKeysToCheck(keycode))
            if (GetKey(code))
                return true;
        return false;
    }

    private static bool Hold(Keys key)
    {
        KeyPair pair = GetKeyCode(key);
        return Hold(pair.GetPrimary()) || Hold(pair.GetSecondary());
    }

    private static bool GetKey(KeyCode code)
    {
        // TODO: deal with exceptions
        code = GetTrueKey(code);

        return Input.GetKey(code);
    }
    public static bool DownKeyHold()
    {
        return Hold(Keys.Down);
    }
    public static bool UpKeyHold()
    {
        return Hold(Keys.Up);
    }
    public static bool RightKeyHold()
    {
        return Hold(Keys.Right);
    }
    public static bool LeftKeyHold()
    {
        return Hold(Keys.Left);
    }
    //-----------------------------------------------------------------------
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

    /*
    ============= Key Piping =============
    */
    // all keycodes that need to be piped into a different code
    private static Dictionary<KeyCode, KeyCode> falseCodes = new Dictionary<KeyCode, KeyCode>
    {
        {KeyCode.Alpha0, KeyCode.Keypad0},
        {KeyCode.Alpha1, KeyCode.Keypad1},
        {KeyCode.Alpha2, KeyCode.Keypad2},
        {KeyCode.Alpha3, KeyCode.Keypad3},
        {KeyCode.Alpha4, KeyCode.Keypad4},
        {KeyCode.Alpha5, KeyCode.Keypad5},
        {KeyCode.Alpha6, KeyCode.Keypad6},
        {KeyCode.Alpha7, KeyCode.Keypad7},
        {KeyCode.Alpha8, KeyCode.Keypad8},
        {KeyCode.Alpha9, KeyCode.Keypad9},
        {KeyCode.KeypadPeriod, KeyCode.Period},
        {KeyCode.KeypadDivide, KeyCode.Slash},
        {KeyCode.KeypadMultiply, KeyCode.Asterisk},
        {KeyCode.KeypadMinus, KeyCode.Minus},
        {KeyCode.KeypadPlus, KeyCode.Plus},
        {KeyCode.KeypadEnter, KeyCode.Return},
        {KeyCode.KeypadEquals, KeyCode.Equals},
        {KeyCode.RightShift, KeyCode.LeftShift},
        {KeyCode.RightControl, KeyCode.LeftControl},
        {KeyCode.RightAlt, KeyCode.LeftAlt}
    };

    public static KeyCode GetTrueKey(KeyCode code)
    {
        if(falseCodes.ContainsKey(code)) return falseCodes[code];
        return code;
    }

    public static List<KeyCode> GetKeysToCheck(KeyCode code)
    {
        List<KeyCode> ret = new List<KeyCode> {code};

        foreach(KeyValuePair<KeyCode, KeyCode> entry in falseCodes)
            if(entry.Value == code)
                ret.Add(entry.Key);

        return ret;
    }

    public static Dictionary<KeyCode, string> validCodeToString {get; private set;} = new Dictionary<KeyCode, string> ()
    {
        {KeyCode.Backspace, "Backspace"},
        {KeyCode.Delete, "Delete"},
        {KeyCode.Tab, "Tab"},
        {KeyCode.Clear, "Clear"},
        {KeyCode.Return, "Enter"},
        {KeyCode.Escape, "Esc"},
        {KeyCode.Space, "Space"},
        {KeyCode.Keypad0, "0"},
        {KeyCode.Keypad1, "1"},
        {KeyCode.Keypad2, "2"},
        {KeyCode.Keypad3, "3"},
        {KeyCode.Keypad4, "4"},
        {KeyCode.Keypad5, "5"},
        {KeyCode.Keypad6, "6"},
        {KeyCode.Keypad7, "7"},
        {KeyCode.Keypad8, "8"},
        {KeyCode.Keypad9, "9"},
        {KeyCode.UpArrow, "↑"},
        {KeyCode.DownArrow, "↓"},
        {KeyCode.RightArrow, "←"},
        {KeyCode.LeftArrow, "→"},
        {KeyCode.F1, "F1"},
        {KeyCode.F2, "F2"},
        {KeyCode.F3, "F3"},
        {KeyCode.F4, "F4"},
        {KeyCode.F5, "F5"},
        {KeyCode.F6, "F6"},
        {KeyCode.F7, "F7"},
        {KeyCode.F8, "F8"},
        {KeyCode.F9, "F9"},
        {KeyCode.F10, "F10"},
        {KeyCode.F11, "F11"},
        {KeyCode.F12, "F12"},
        {KeyCode.F13, "F13"},
        {KeyCode.F14, "F14"},
        {KeyCode.F15, "F15"},
        {KeyCode.Exclaim, "!"},
        {KeyCode.DoubleQuote, "\""},
        {KeyCode.Hash, "#"},
        {KeyCode.Dollar, "$"},
        {KeyCode.Percent, "%"},
        {KeyCode.Ampersand, "^"},
        {KeyCode.Quote, "\""},
        {KeyCode.LeftParen, "("},
        {KeyCode.RightParen, ")"},
        {KeyCode.Asterisk, "*"},
        {KeyCode.Plus, "+"},
        {KeyCode.Comma, ","},
        {KeyCode.Minus, "-"},
        {KeyCode.Period, "."},
        {KeyCode.Slash, "/"},
        {KeyCode.Colon, ":"},
        {KeyCode.Semicolon, ";"},
        {KeyCode.Less, "<"},
        {KeyCode.Equals, "="},
        {KeyCode.Greater, ">"},
        {KeyCode.Question, "?"},
        {KeyCode.At, "Alt"},
        {KeyCode.LeftBracket, "["},
        {KeyCode.Backslash, "\\"},
        {KeyCode.RightBracket, "]"},
        {KeyCode.Caret, "^"},
        {KeyCode.Underscore, "_"},
        {KeyCode.BackQuote, "`"},
        {KeyCode.A, "A"},
        {KeyCode.B, "B"},
        {KeyCode.C, "C"},
        {KeyCode.D, "D"},
        {KeyCode.E, "E"},
        {KeyCode.F, "F"},
        {KeyCode.G, "G"},
        {KeyCode.H, "H"},
        {KeyCode.I, "I"},
        {KeyCode.J, "J"},
        {KeyCode.K, "K"},
        {KeyCode.L, "L"},
        {KeyCode.M, "M"},
        {KeyCode.N, "N"},
        {KeyCode.O, "O"},
        {KeyCode.P, "P"},
        {KeyCode.Q, "Q"},
        {KeyCode.R, "R"},
        {KeyCode.S, "S"},
        {KeyCode.T, "T"},
        {KeyCode.U, "U"},
        {KeyCode.V, "V"},
        {KeyCode.W, "W"},
        {KeyCode.X, "X"},
        {KeyCode.Y, "Y"},
        {KeyCode.Z, "Z"},
        {KeyCode.LeftCurlyBracket, "{"},
        {KeyCode.Pipe, "|"},
        {KeyCode.RightCurlyBracket, "}"},
        {KeyCode.Tilde, "~"},
        {KeyCode.Numlock, "Num Lock"},
        {KeyCode.CapsLock, "Caps Lock"},
        {KeyCode.ScrollLock, "Scroll Lock"},
        {KeyCode.RightShift, "Shift"},
        {KeyCode.LeftShift, "Shift"},
        {KeyCode.LeftControl, "Ctrl"},
        {KeyCode.LeftAlt, "Alt"},
        {KeyCode.Mouse0, "L Click"},
        {KeyCode.Mouse1, "R Click"}
    };

    public static string GetKeyString(Keys key)
    {
        return validCodeToString[keycodes[key].GetPrimary()];
    }

    public static string GetKeyCodeString(KeyCode code)
    {
        if(!validCodeToString.ContainsKey(code)) return "";
        return validCodeToString[code];
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

    public KeyPair(KeyPair cpy)
    {
        primary = cpy.primary;
        secondary = cpy.secondary;
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

    public KeyPair RemoveCode(KeyCode code)
    {
        if(!UsesCode(code))
        {
            Debug.LogError("tried to remove KeyCode from a KeyPair that didn't have that code");
            return null;
        }

        KeyPair ret = new KeyPair(this);

        if(code == ret.primary)
        {
            ret.primary = ret.secondary;
        }

        ret.secondary = KeyCode.None;
        return ret;
    }

    public KeyPair AddCode(KeyCode code)
    {
        if(UsesCode(code))
        {
            Debug.LogError("tried to add KeyCode from a KeyPair that already had that code");
            return null;
        }

        KeyPair ret = new KeyPair(this);

        if(ret.secondary != KeyCode.None)
        {
            ret.primary = ret.secondary;
        }

        ret.secondary = code;
        return ret;
    }

    public bool IsSingle()
    {
        return secondary == KeyCode.None;
    }
}
