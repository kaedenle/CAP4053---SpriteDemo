using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MazeManager : MonoBehaviour
{
    public GameObject[] specials;
    [Range(1,15)] public int pathLength = 6;
    public GameObject[] decorations;
    public int averageNumberOfDecorations = 4;
    private MazeHints hints;

    public enum Direction
    {
        Left,
        Up,
        Right,
        Down
    }

    // constants
    const int directions = 3,
              seed = 42_069_420;

    private static System.Random rand;
    private static Maze maze;
    private static Stack<Maze> path;

    void Awake()
    {
        if(maze == null)
        {
            rand = new System.Random(seed);
            maze = GenerateMaze();
            path = new Stack<Maze>();
        }

        Debug.Log("entered rest of Awake()");
        if(path.Count <= 0)
            path.Push(maze);    // start in the first room  

        hints = FindObjectOfType<MazeHints>();

        SetupRoom(GetCurrentRoom());
    }

    public Maze GetCurrentRoom()
    {
        return path.Peek();
    }

    public void TakeStairs(Direction dir)
    {
        // go back
        if(dir == Direction.Down)
        {
            if(path.Count <= 1)
            {
                ExitMaze(false);
                return;
            }

            // do stuff
            path.Pop();
            RestartRoom();

            return;
        }

        // go to the next room
        Maze cur = GetCurrentRoom();

        // is the end room
        if(cur.IsTerminal())
        {
            EndOfPath();
            return;
        }

        path.Push(cur.GetNext((int)dir));
        RestartRoom();
    }

    public void RestartRoom()
    {
        ScenesManager.ReloadScene();
    }

    public void SetupRoom(Maze cur)
    {
        // set up the decorations
        bool[] dec_state = cur.GetDecorations();

        if(cur.NotSetup())
        {
            // set up decorations
            double prob = averageNumberOfDecorations / (double) decorations.Length;
            dec_state = new bool[decorations.Length];

            for(int i = 0; i < dec_state.Length; i++)
                if(rand.NextDouble() < prob)
                    dec_state[i] = true;
            
            cur.SetDecorations(dec_state);

            // set up hint type
            cur.SetHints(rand.Next(0, directions));
        }

        for(int i = 0; i < decorations.Length; i++)
        {
            if(decorations[i] == null) continue;

            decorations[i].SetActive(dec_state[i]);
        }

        // set up hints
        SetupHints(cur);

        // set up specials
        // (specials default should be false)
        if(cur.IsTerminal())
        {
            for(int special = 0; special < specials.Length; special++)
                if(cur.IsOnPath(special) && CastleLevelManager.ObtainedPrereqs(special) && specials[special] != null)
                    specials[special].SetActive(true);
        }
    }

    public void SetupHints(Maze cur)
    {
        hints.RemoveAll();

        if(cur.IsSpecial())
        {
            // remove all hints
            return;
        }

        // set the room banner
        int bannerType = cur.GetHint();
        hints.SetBanner(bannerType);

    }

    public void EndOfPath()
    {
        // didn't satisfy special sequence
        ExitMaze(true);
    }

    // exits back to the arena
    public void ExitMaze(bool status)
    {
        CastleLevelManager.SetMazeStatus(status);
        ScenesManager.LoadScene(ScenesManager.AllScenes.CastleArena);
    }

    // generates all the paths of the maze
    public Maze GenerateMaze()
    {
        // make trie maze
        Maze root = new Maze(pathLength + 1);
        
        // make the special paths
        for(int special = 0; special < specials.Length; special ++)
        {
            Maze cur = root;
            cur.SetOnPath(special);

            while(!cur.IsTerminal())
            {
                int dir = rand.Next(0, directions);
                Debug.Log("dir is " + dir + " for path " + special);
                cur = cur.GetNext(dir);
                cur.SetOnPath(special);
            }
        }

        // set up decorations
        


        return root;
    }
}
