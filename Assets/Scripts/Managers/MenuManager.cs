using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void NewGame()
    {
        ScenesManager.LoadNewGame();
    }

    public void PlayDemo()
    {
        ScenesManager.StartDemo();
    }

    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
