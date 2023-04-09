using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class KeybindingsMenuManager : MonoBehaviour
{
    public KeybindingButtonInfo[] buttonInformation;
    public GameObject UIAlert;
    public GameObject confirmBindingsPopup;
    public GameObject backPopup;
    public GameObject optionsMenu;
    public GameObject thisMenu;
    private float reactionDelay = 0.1F;
    private bool eventDelay = false;
    private bool inBindingProcess;
    private bool delayFinished;
    private InputManager.Keys bind;
    private static Dictionary<InputManager.Keys, KeyPair> menuKeys;
    private bool saved;
    private bool started = false;

    void Start()
    {
        Setup();
        started = true;
    }

    void OnEnable()
    {
        if(started)
        {
            Setup();
        }
    }

    void Setup()
    {
        saved = true;
        inBindingProcess = false;
        delayFinished = false;

        if(UIAlert != null)
            UIAlert.SetActive(false);
        
        if(backPopup != null)
            backPopup.SetActive(false);
        
        if(confirmBindingsPopup != null)
            confirmBindingsPopup.SetActive(false);

        menuKeys = new Dictionary<InputManager.Keys, KeyPair>(InputManager.GetBindings());
        LoadText();
    }

    void OnGUI()
    {
        if(inBindingProcess && delayFinished)
        {
            Event e = Event.current;
            // case out mouse0-mouse6
            Debug.Log("event: " + e.ToString() + " with keycode " + e.keyCode);

            if (e.keyCode != KeyCode.None)
            {
                AttemptKeyBind(e.keyCode);
            }

            else if(e.isMouse)
            {
                if(Input.GetKeyDown(KeyCode.Mouse0))
                    AttemptKeyBind(KeyCode.Mouse0);
                else if(Input.GetKeyDown(KeyCode.Mouse1))
                    AttemptKeyBind(KeyCode.Mouse1);
            }
            
        }
    }

    void LoadText()
    {
        foreach(KeybindingButtonInfo info in buttonInformation)
        {
            if(!menuKeys.ContainsKey(info.key)) continue;

            KeyPair bindings = menuKeys[info.key];
            // Debug.Log("found the keybindingsinfo for key " + info.key.ToString());

            info.GetPrimaryText().text = KeyCodeToText(bindings.GetPrimary());
            // Debug.Log("text of primary is " + info.GetPrimaryText().text);

            if(bindings.GetSecondary() == KeyCode.None)
                info.GetSecondaryObject().SetActive(false);
            else
            {
                info.GetSecondaryObject().SetActive(true);
                info.GetSecondaryText().text = KeyCodeToText(bindings.GetSecondary());
            }
        }
    }

    public void AttemptKeyBind(KeyCode key)
    {
        Debug.Log("attempting keybind with key " + key);

        if(UpdateKeyWithCode(key, bind))
        {
            saved = false;
            LoadText();
        }
        
        // shut down the key bindings process
        if(UIAlert != null)
            UIAlert.SetActive(false);

        inBindingProcess = false;
        StartCoroutine(delayEvent());
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
            
            pair.RemoveCode(key);
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
        pair.AddCode(key);
        Debug.Log("applying code " + key + " to " + bind);
        return true;
    }

    IEnumerator delayReaction()
    {
        yield return new WaitForSecondsRealtime(reactionDelay);
        delayFinished = true;
    }

    IEnumerator delayEvent()
    {
        yield return new WaitForSecondsRealtime(0.5F);
        eventDelay = false;
    }

    public void SaveBindings()
    {
        InputManager.SaveNewBindings(menuKeys);
        saved = true;
    }

    public void SaveButton()
    {
        if(inBindingProcess) return;

        SaveBindings();
    }

    public void BackButton()
    {
        if(saved || backPopup == null)
        {
            optionsMenu.SetActive(true);
            thisMenu.SetActive(false);
        }

        else
        {
            backPopup.SetActive(true);
        }
    }

    public void RevertButton()
    {
        if(inBindingProcess) return;

        if(confirmBindingsPopup != null)
            confirmBindingsPopup.SetActive(true);
        else
            RevertBindings();
    }

    public void RevertBindings()
    {
        InputManager.RevertToDefault();
        menuKeys = new Dictionary<InputManager.Keys, KeyPair>(InputManager.GetBindings());
        LoadText();
    }

    public void TriggerBinding(InputManager.Keys bindToUpdate)
    {
        if(inBindingProcess || eventDelay) return;
        
        delayFinished = false;
        inBindingProcess = true;
        eventDelay = true;
        StartCoroutine(delayReaction());

        if(UIAlert != null)
            UIAlert.SetActive(true);
        
        bind = bindToUpdate;
    }

    public void UpdateUpKey()
    {
        TriggerBinding(InputManager.Keys.Up);
    }
    public void UpdateDownKey()
    {
        TriggerBinding(InputManager.Keys.Down);
    }

    public void UpdateLeftKey()
    {
        TriggerBinding(InputManager.Keys.Left);
    }

    public void UpdateRightKey()
    {
        TriggerBinding(InputManager.Keys.Right);
    }

    public void UpdateContinueKey()
    {
        TriggerBinding(InputManager.Keys.Continue);
    }

    public void UpdateInteractKey()
    {
        TriggerBinding(InputManager.Keys.Interact);
    }

    public void UpdateHit1Key()
    {
        TriggerBinding(InputManager.Keys.Hit1);
    }

    public void UpdateHit2Key()
    {
        TriggerBinding(InputManager.Keys.Hit2);
    }

    public void UpdateEquipKey()
    {
        TriggerBinding(InputManager.Keys.Equip);
    }

    public void UpdateSwapKey()
    {
        TriggerBinding(InputManager.Keys.Swap);
    }

    public void UpdatePauseKey()
    {
        TriggerBinding(InputManager.Keys.Pause);
    }


    public static string KeyCodeToText(KeyCode code)
    {
        if(codeToString.ContainsKey(code)) return codeToString[code];
        return code.ToString();
    }

    private static Dictionary<KeyCode, string> codeToString = new Dictionary<KeyCode, string> ()
    {
        {KeyCode.UpArrow, "↑"},
        {KeyCode.DownArrow, "↓"},
        {KeyCode.LeftArrow, "←"},
        {KeyCode.RightArrow, "→"},
        // {KeyCode.Alpha0, "0"},
        // {KeyCode.Alpha1, "1"},
        // {KeyCode.Alpha2, "2"},
        // {KeyCode.Alpha3, "3"},
        // {KeyCode.Alpha4, "4"},
        // {KeyCode.Alpha5, "5"},
        // {KeyCode.Alpha6, "6"},
        // {KeyCode.Alpha7, "7"},
        // {KeyCode.Alpha8, "8"},
        // {KeyCode.Alpha9, "9"},
        {KeyCode.Mouse0, "L-Click"},
        {KeyCode.Mouse1, "R-Click"},
        {KeyCode.Escape, "Esc"},
        {KeyCode.LeftShift, "Lshift"},
        {KeyCode.RightShift, "Rshift"}
    };
}

[System.Serializable]
public class KeybindingButtonInfo
{
    public InputManager.Keys key;
    public GameObject primaryKeyTextObject;
    public GameObject secondaryKeyTextObject;

    private TMP_Text primaryText;
    private TMP_Text secondaryText;

    public TMP_Text GetPrimaryText()
    {
        if(primaryKeyTextObject == null) return null;
        if(primaryText == null)
        {
            primaryText = primaryKeyTextObject.GetComponent<TMP_Text>();
        }

        return primaryText;
    }

    public TMP_Text GetSecondaryText()
    {
        if(secondaryKeyTextObject == null) return null;
        if(secondaryText == null)
        {
            secondaryText = secondaryKeyTextObject.GetComponent<TMP_Text>();
        }

        return secondaryText;
    }

    public GameObject GetSecondaryObject()
    {
        return secondaryKeyTextObject;
    }
}
