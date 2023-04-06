using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// no current logic to deal with end of game
public class GameState
{
    public int current_level;
    public int current_level_state;

    private static string levelString = "CurrentLevel";
    private static string stateString = "CurrentState";


    public GameState (int level, int state)
    {
        current_level = level;
        current_level_state = state;
    } 

    public static GameState LoadGame()
    {
        if(!HasSavedGame())
        {
            return new GameState(0, 0);
        }

        int level = PlayerPrefs.GetInt(levelString);
        int state = PlayerPrefs.GetInt(stateString);

        return new GameState(level, state);
    }

    public static bool HasSavedGame()
    {
        if(! (PlayerPrefs.HasKey(levelString) && PlayerPrefs.HasKey(stateString)))
            return false;

        int boss_level_number = 4;   
        return LoadLevel() <= boss_level_number;
    }

    public static void HardReset()
    {
        GameState newGame = new GameState(0, 0);
        newGame.Save();
    }

    public void IncrementStateAndSave()
    {
        if(current_level_state == 1)
        {
            current_level++;
            current_level_state = 0;
        }

        else
        {
            current_level_state++;
        }

        Save();
    }

    public void Save()
    {
        PlayerPrefs.SetInt(levelString, current_level);
        PlayerPrefs.SetInt(stateString, current_level_state);

        Debug.Log("Saving game with level=" + current_level + " and state=" + current_level_state);
        PlayerPrefs.Save();
    }

    public static int LoadLevel()
    {
        return PlayerPrefs.GetInt(levelString);
    }

    public static int LoadState()
    {
        return PlayerPrefs.GetInt(stateString);
    }

    public int GetLevel()
    {
        return current_level;
    }

    public int GetState()
    {
        return current_level_state;
    }
}
