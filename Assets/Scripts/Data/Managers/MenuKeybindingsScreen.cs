using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(KeybindingsManager))]
public class MenuKeybindingsScreen : MonoBehaviour
{
    private KeybindingsManager manager;
    public BindingButton[] buttonInformation;
    public GameObject UIAlert;  // UI that tells the player to "press a key to bind"
    public GameObject confirmBindingsPopup; // UI that asks the player if they really want to revert the bindings
    public GameObject backPopup; // UI that tells the player the bindings aren't saved yet, and asks if they would really like to quit
    public GameObject optionsMenu;
    public GameObject thisMenu;
    private float reactionDelay = 0.1F;
    private bool eventDelay = false;
    private bool inBindingProcess;
    private bool delayFinished;
    private InputManager.Keys bind;
    private bool saved;
    private bool started = false;

    // grab the backend manager
    void Awake()
    {
        manager = GetComponent<KeybindingsManager>();
    }

    // setup on start or enable
    void Start()
    {
        Setup();
        started = true;
    }

    void OnEnable()
    {
        if(started)
        {
            Debug.Log("setup");
            Setup();
        }
    }

    // set up bindings screen to the default configuration
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

        LoadText();
        LockMovementKeys();
    }

    // lock the movement keys in place (will have buggy behavior if this is not the case)
    void LockMovementKeys()
    {
        foreach(BindingButton binding in buttonInformation)
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

    // detect button + mouse presses
    void OnGUI()
    {
        if(inBindingProcess && delayFinished)
        {
            Event e = Event.current;
            // case out mouse0-mouse6

            if (e.keyCode != KeyCode.None)
            {
                if(GeneralFunctions.IsDebug()) Debug.Log("event: " + e.ToString() + " with keycode " + e.keyCode);
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

    // loads all the text of the actual bindings
    void LoadText()
    {
        foreach(BindingButton info in buttonInformation)
        {
            info.InitButton();   // setup the info given the key gameobject (section)

            if(!manager.GetKeys().ContainsKey(info.key)) continue;

            KeyPair bindings = manager.GetKeys()[info.key];
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

    // try to bind a new keycode
    public void AttemptKeyBind(KeyCode key)
    { 
        if(manager.AttemptKeyBind(key, bind))
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

    // have some delay between when the player starts a binding and when the script can detect a button/mouse press
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

    // set the bindings back to default
    public void RevertBindings()
    {
        manager.RevertBindings();
        LoadText();
        saved = true;
    }

    // trigger the binding process
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

    // get the actual text of a keycode
    public static string KeyCodeToText(KeyCode code)
    {
        if(InputManager.validCodeToString.ContainsKey(code)) return InputManager.validCodeToString[code];
        return code.ToString();
    }

    /* 
    =========== Button Effects ============
    */

    public void SaveButton()
    {
        if(inBindingProcess) return;

        manager.SaveBindings();
        saved = true;
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


}

// class for holding button information
[System.Serializable]
public class BindingButton
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
