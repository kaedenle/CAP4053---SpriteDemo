using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

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

        LockMovementKeys();
    }

    void LockMovementKeys()
    {
        foreach(KeybindingButtonInfo binding in buttonInformation)
        {
            if(binding.key == InputManager.Keys.Up || binding.key == InputManager.Keys.Down ||
               binding.key == InputManager.Keys.Left || binding.key == InputManager.Keys.Right)
            {
                Button button = binding.ParentSection.GetComponentInChildren<Button>();
                button.interactable = false;
                button.enabled = false;
                button.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0); // set transparent color
            }
        }
    }

    void OnGUI()
    {
        if(inBindingProcess && delayFinished)
        {
            Event e = Event.current;
            // case out mouse0-mouse6
            if(GeneralFunctions.IsDebug()) Debug.Log("event: " + e.ToString() + " with keycode " + e.keyCode);

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
            info.InitButton();   // setup the info given the key gameobject (section)

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
        // get the piped keycode
        key = InputManager.GetTrueKey(key);

        // check that this is a valid key to even try to bind
        if(!InputManager.validCodeToString.ContainsKey(key)) return;

        if(GeneralFunctions.IsDebug()) Debug.Log("attempting keybind with key " + key);

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
        if(GeneralFunctions.IsDebug()) Debug.Log("applying code " + key + " to " + bind);
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
        if(InputManager.validCodeToString.ContainsKey(code)) return InputManager.validCodeToString[code];
        return code.ToString();
    }
}

[System.Serializable]
public class KeybindingButtonInfo
{
    public InputManager.Keys key;
    public GameObject ParentSection;
    private GameObject primary, secondary;
    private TMP_Text primaryText, secondaryText;

    public void InitButton()
    {
        if(ParentSection == null)
        {
            Debug.LogError("Key " + key.ToString() + " had no section attached in the keybindings menu");
            return;
        }

        primary = GeneralFunctions.GetChildByName(ParentSection, "Primary");
        secondary = GeneralFunctions.GetChildByName(ParentSection, "Secondary");

        if(primary == null || secondary == null)
        {
            Debug.LogError("Key " + key.ToString() + " could not find primary or secondary child gameobject");
            return;
        }

        primaryText = primary.GetComponentInChildren<TMP_Text>();
        secondaryText = secondary.GetComponentInChildren<TMP_Text>();
    }

    public TMP_Text GetPrimaryText()
    {
        return primaryText;
    }

    public TMP_Text GetSecondaryText()
    {
        return secondaryText;
    }

    public GameObject GetSecondaryObject()
    {
        return secondary;
    }
}
