using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class KeybindingsManager : MonoBehaviour
{
    private static Dictionary<InputManager.Keys, KeyPair> menuKeys;

    void Awake()
    {
        menuKeys = new Dictionary<InputManager.Keys, KeyPair>(InputManager.GetBindings());
    }

    void OnDisable()
    {
        menuKeys = new Dictionary<InputManager.Keys, KeyPair>(InputManager.GetBindings());
        Debug.Log("disabled");
    }

    public bool AttemptKeyBind(KeyCode key, InputManager.Keys bind)
    { 
        // get the piped keycode
        key = InputManager.GetTrueKey(key);

        // check that this is a valid key to even try to bind
        if(!InputManager.validCodeToString.ContainsKey(key)) return false;

        if(GeneralFunctions.IsDebug()) 
            Debug.Log("attempting keybind with key " + key);

        if(UpdateKeyWithCode(key, bind))
        {
            Debug.Log("keybind success!");
            return true;
        }

        else
            return false;
    }

    public static bool UpdateKeyWithCode(KeyCode key, InputManager.Keys bind)
    {
        KeyPair pair = menuKeys[bind];

        // if the key is already using this keycode
        if(pair.UsesCode(key))
        {
            // will remove the only binding
            if(pair.IsSingle())
                return false;
            
            pair = menuKeys[bind] = pair.RemoveCode(key);
            return true;
        }

        // determine whether it intersects with other keyes (exception: continue)
        if(bind != InputManager.Keys.Continue)
            foreach(KeyValuePair<InputManager.Keys, KeyPair> entry in menuKeys)
            {
                // case out continue key (doesn't matter if there's a collision)
                if(entry.Key == InputManager.Keys.Continue) continue;
                // case out your own key (shouldn't matter, but doesn't hurt to check)
                if(entry.Key == bind) continue;

                // check if there's a collision
                if(entry.Value.UsesCode(key))
                    return false;
            }

        // everything was fine
        menuKeys[bind] = pair = pair.AddCode(key);
        if(GeneralFunctions.IsDebug()) Debug.Log("applying code " + key + " to " + bind);

        return true;
    }

    public void SaveBindings()
    {
        InputManager.SaveNewBindings(menuKeys);
    }
    
    public void RevertBindings()
    {
        InputManager.RevertToDefault();
        menuKeys = new Dictionary<InputManager.Keys, KeyPair>(InputManager.GetBindings());
    }

    // returns copy of key mappings
    public Dictionary<InputManager.Keys, KeyPair> GetKeys()
    {
        return new Dictionary<InputManager.Keys, KeyPair> (menuKeys);
    }
}