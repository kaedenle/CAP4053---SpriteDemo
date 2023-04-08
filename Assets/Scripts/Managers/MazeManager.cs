using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MazeManager : MonoBehaviour
{
    public GameObject[] specials;
    public GameObject decorationsContainer;
    private GameObject[] decorations;
    public int averageNumberOfDecorations = 4;

    // The length of the maze path (number of rooms traversed until exit)
    private int pathLength = 5;

    // maze hints object
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

        if(path.Count <= 0)
            path.Push(maze);    // start in the first room  

        hints = FindObjectOfType<MazeHints>();

        if(decorationsContainer != null)
        {
            decorations = new GameObject[decorationsContainer.transform.childCount];
            int idx = 0;
            foreach(Transform tr in decorationsContainer.transform)
                decorations[idx++] = tr.gameObject;
        }
        else
            decorations = new GameObject[0];
    }

    void Start()
    {
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
        if(cur.NotSetup())
        {
            cur.SetupRoom(rand, decorations.Length, averageNumberOfDecorations / (double) decorations.Length);
        }

        // set up the decorations
        bool[] dec_state = cur.GetDecorations();

        for(int i = 0; i < decorations.Length; i++)
        {
            if(decorations[i] == null) continue;

            decorations[i].SetActive(dec_state[i]);
        }

        // set up hints
        hints.SetHints(cur);

        // set up specials
        // (specials default should be false)
        if(cur.IsTerminal())
        {
            for(int special = 0; special < specials.Length; special++)
                if(cur.IsOnPath(special) && CastleLevelManager.ObtainedPrereqs(special) && specials[special] != null)
                    specials[special].SetActive(true);
        }
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
        path.Clear();
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
