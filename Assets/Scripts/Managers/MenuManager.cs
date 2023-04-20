using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject LoadButton;
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

    void Start()
    {
        if(LoadButton != null && !GameData.GetInstance().HasLoadData())
            DisableButton(LoadButton);
    }
}
