using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MazeManager : MonoBehaviour
{
    public GameObject[] decorations;
    public int averageNumberOfDecorations = 4;
    [Range(1,15)] public int pathLength = 6;

    public enum Direction
    {
        Left,
        Up,
        Right,
        Down
    }

    // constants
    const int specials = 3,
              directions = 3,
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
            // do stuff

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

        if(dec_state == null)
        {
            double prob = averageNumberOfDecorations / (double) decorations.Length;
            dec_state = new bool[decorations.Length];

            for(int i = 0; i < dec_state.Length; i++)
                if(rand.NextDouble() < prob)
                    dec_state[i] = true;
            
            cur.SetDecorations(dec_state);
        }

        for(int i = 0; i < decorations.Length; i++)
        {
            if(decorations[i] == null) continue;

            decorations[i].SetActive(dec_state[i]);
        }

        // set up the proper hints
    }

    public void EndOfPath()
    {
        // if satisfied path sequence


        // didn't satisfy special sequence
        CastleLevelManager.SetMazeStatus(true);
        ScenesManager.LoadScene(ScenesManager.AllScenes.CastleArena);
    }
    
    public Maze GenerateMaze()
    {
        // make trie maze
        Maze root = new Maze(pathLength + 1);
        
        // make the special paths
        for(int special = 0; special < specials; special ++)
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
