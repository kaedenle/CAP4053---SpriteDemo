using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // button behavior for New Game
    public void NewGame()
    {
        GameState.HardReset();
        ScenesManager.LoadScene( ScenesManager.AllScenes.StartCutScene );
    }

    // button behavior for Load Game
    public void LoadGame()
    {
        if(GameState.HasSavedGame())
            StartGame();
        
        // do nothing if no game saved
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
