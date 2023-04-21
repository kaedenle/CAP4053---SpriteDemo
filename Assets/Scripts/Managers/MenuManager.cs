using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject LoadButton, levelSelectButton;
    public TMP_ColorGradient disabledColor;

    // button behavior for New Game
    public void NewGame()
    {
        GameData.GetInstance().ResetData();
        ScenesManager.LoadScene( ScenesManager.AllScenes.StartCutScene );
    }

    // button behavior for Load Game
    public void LoadGame()
    {
        if(GameData.GetInstance().HasLoadData())
            StartGame();
        
        // do nothing if no game saved (or maybe play a fail sound)
    }

    // starts the game from the menu
    public void StartGame()
    {
        // reset all level variables
        LevelManager.FullReset();

        // grab the current game
        GameData.GetInstance().RevertToSave();
    }

    // quit button behavior
    public void Quit()
    {
        if(GeneralFunctions.IsDebug()) Debug.Log("QUIT");
        Application.Quit();
    }

    public void DisableButton(GameObject obj)
    {
        Button button = obj.GetComponent<Button>();
        button.interactable = false;
        button.enabled = false;

        // set transparent color
        obj.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        if(disabledColor != null) obj.GetComponentInChildren<TMP_Text>().colorGradientPreset = disabledColor;
    }

    void Awake()
    {
        levelSelectButton.SetActive(false);
    }

    void Start()
    {
        if(LoadButton != null && !GameData.GetInstance().HasLoadData())
            DisableButton(LoadButton);
    }

    /*
    ========= Level Select & Dev Tools =========
    */

    public void LevelSelectButton(int level)
    {
        GameData.GetInstance().ResetData();
        LevelManager.FullReset();
        GameData.GetInstance().SetLevel(level);
        GameData.GetInstance().SaveAfterSceneChange();
        ScenesManager.LoadScene(ScenesManager.AllScenes.CentralHub);
    }

    private string secretKey = "DEBUG";
    private string currentString = "";
    private bool delayDone = true;

    void OnGUI()
    {
        if(!delayDone) return;

        Event e = Event.current;

        if (e.keyCode != KeyCode.None)
        {
            delayDone = false;
            StartCoroutine(DelayTyping());
            if(GeneralFunctions.IsDebug()) Debug.Log("event: " + e.ToString() + " with keycode " + e.keyCode);
            
            currentString += InputManager.GetKeyCodeString(e.keyCode);

            if(currentString.Length > secretKey.Length) 
                currentString = currentString.Substring(0, secretKey.Length);
            
            if(currentString.Equals(secretKey))
                levelSelectButton.SetActive(true);

            if(GeneralFunctions.IsDebug()) Debug.Log("current string is " + currentString);
        }
    }

    IEnumerator DelayTyping()
    {
        yield return new WaitForSeconds(0.1F);
        delayDone = true;
    }
}
