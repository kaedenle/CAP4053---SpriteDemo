using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    // button behavior for New Game
    public void NewGame()
    {
        GameData.GetInstance().ResetData();
        // GameData.GetInstance().SaveAfterSceneChange();
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

        // GameState game = GameState.LoadGame();
        // int state = game.GetState();

        // if(state == 0) 
        //     ScenesManager.LoadScene(ScenesManager.AllScenes.CentralHub);
        // else
        // {
        //     HubManager.LoadPhase(game.GetLevel());
        // }
    }

    // quit button behavior
    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
