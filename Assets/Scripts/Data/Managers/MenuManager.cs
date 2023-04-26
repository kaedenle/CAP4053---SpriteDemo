using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject LoadButton;
    public TMP_ColorGradient disabledColor;

    public GameObject levelSelectButton;
    public GameObject creditsButton;
    public GameObject areYouSurePopup;
    public GameObject mainPage;
    public GameObject mainTheme;

    private GameData.Difficulty chosen_difficulty;
    private int chosen_level;
    ScenesManager.AllScenes chosen_scene;

    // button behavior for New Game
    public void NewGame()
    {
        chosen_level = 0;
        chosen_scene = ScenesManager.AllScenes.StartCutScene;
    }

    public void SelectDifficulty(int difficulty)
    {
        chosen_difficulty = (GameData.Difficulty) difficulty;

        if(GameData.GetInstance().HasLoadData())
        {
            areYouSurePopup.SetActive(true);
        }

        else
        {
            ResetAndStart();
        }
    }

    // actually start the game
    public void ResetAndStart()
    {
        // reset all level variables
        LevelManager.FullReset();
        GameData data = GameData.GetInstance();
        data.ResetData();

        data.SetDifficulty(chosen_difficulty);
        data.SetLevel(chosen_level);
        data.SetScene(chosen_scene);

        StartGame();
    }

    // button behavior for Load Game
    public void LoadGame()
    {
        if(GameData.GetInstance().HasLoadData())
        {
            GameData.GetInstance().LoadData();
            StartGame();
        }
        // do nothing if no game saved (or maybe play a fail sound)
    }

    // starts the game from the menu
    public void StartGame()
    {
        // stop the music
        if(mainTheme != null)
            mainTheme.GetComponent<AudioSource>().Stop();

        // grab the current game
        LevelManager.FullReset();
        GameData.GetInstance().RevertToSave();
    }

    // quit button behavior
    public void Quit()
    {
        if(GeneralFunctions.IsDebug()) Debug.Log("QUIT");
        Application.Quit();
    }

    public void RollCredits()
    {
        ScenesManager.LoadScene(ScenesManager.AllScenes.Credits);
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

        // always enable mouse on menu
        Cursor.visible = true;
    }

    void Start()
    {
        if(LoadButton != null && !GameData.GetInstance().HasLoadData())
            DisableButton(LoadButton);

        if(!GameData.GetInstance().CompletedGame())
            creditsButton.SetActive(false);
    }

    /*
    ========= Level Select & Dev Tools =========
    */

    public void LevelSelectButton(int level)
    {
        // GameData.GetInstance().ResetData();
        // LevelManager.FullReset();
        // GameData.GetInstance().SetLevel(level);
        // GameData.GetInstance().SetScene(ScenesManager.AllScenes.CentralHub);

        chosen_level = level;
        chosen_scene = ScenesManager.AllScenes.CentralHub;
    }

    private string secretKey = "DEBUG";
    private string currentString = "";
    private bool delayDone = true;

    void OnGUI()
    {
        if(!delayDone || !mainPage.activeSelf ) return;

        Event e = Event.current;

        if (e.keyCode != KeyCode.None)
        {
            string typed = InputManager.GetKeyCodeString(e.keyCode);
            if(typed.Length != 1) return; // ignore any special keys or invalid keys
            
            char letter = typed[0];
            if(currentString.Length != 0 && currentString[currentString.Length - 1] == letter) return; // repeat letter

            StartCoroutine(DelayTyping());
            
            currentString += typed;

            while(currentString.Length > secretKey.Length) 
                currentString = currentString.Substring(1);
            
            if(currentString.Equals(secretKey))
                levelSelectButton.SetActive(true);

            if(GeneralFunctions.IsDebug()) Debug.Log("current string is " + currentString);
        }
    }

    IEnumerator DelayTyping()
    {
        delayDone = false;
        yield return new WaitForSeconds(0.05F);
        delayDone = true;
    }
}
