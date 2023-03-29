using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // button behavior for New Game
    public void NewGame()
    {
        GameState.HardReset();
        StartGame();
    }

    // button behavior for Load Game
    public void LoadGame()
    {
        StartGame();
    }

    // starts the game from the menu
    public void StartGame()
    {
        // reset all level variables
        LevelManager.FullReset();

        // grab the current game
        GameState game = GameState.LoadGame();
        int state = game.GetState();

        if(state == 0) 
            ScenesManager.LoadScene(ScenesManager.AllScenes.CentralHub);
        else
        {
            HubManager.LoadPhase(game.GetLevel());
        }
    }

    // quit button behavior
    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
