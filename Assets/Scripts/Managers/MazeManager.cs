using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MazeManager : MonoBehaviour
{
    public GameObject[] specials;
    public GameObject doors;
    public GameObject decorationsContainer;
    private GameObject[] decorations;
    public int averageNumberOfDecorations = 4;
    public GameObject[] startPosition;

    // The length of the maze path (number of rooms traversed until exit)
    private static int pathLength = 4;  // the lower this goes, the more likely a collision will happen

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
    private static Stack<Direction> dirPath;

    void Awake()
    {
        if(maze == null) maze = GenerateMaze();
        if(rand == null) rand = new System.Random(seed);
        if(path == null) path = new Stack<Maze>();

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

    public static bool MazeCreated()
    {
        return maze != null;
    }

    public static void SetMaze(Maze m)
    {
        maze = m;
    }

    public static void SetPath(Stack<Maze> p)
    {
        path = new Stack<Maze>(p);
    }

    void Start()
    {
        SetupRoom(GetCurrentRoom());

        // check if the player went backward
        if(dirPath == null) dirPath = new Stack<Direction>();
        if(dirPath.Count >= 2)
        {
            if(dirPath.Peek() == Direction.Down)
            {
                dirPath.Pop();
                GeneralFunctions.GetPlayer().transform.position = startPosition[(int) dirPath.Peek()].transform.position;
                dirPath.Pop();
            }
        }

        // add checkpoint: reach a special room
        if(path.Count > 0)
        {
            Maze cur = GetCurrentRoom();

            if(cur.IsTerminal())
            {
                int type = GetSpecialType(cur);
                if(type != -1)
                    if(!CastleLevelManager.ObtainedPrereqs(type + 1))
                    {
                        GameData.GetInstance().UpdatePath(path);
                        GameData.GetInstance().SaveCurrentData();
                    }
            }
        }
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
            dirPath.Push(Direction.Down);
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
        dirPath.Push(dir);
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
            GameData.GetInstance().UpdateMaze(maze);
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
            // Debug.Log("is terminal; specials.Length=" + specials.Length);
            for(int special = 0; special < specials.Length; special++)
            {
                // Debug.Log("is on path: " + cur.IsOnPath(special).ToString() + " obtained prereqs: " + CastleLevelManager.ObtainedPrereqs(special).ToString() + " is not null: " + (specials[special] != null).ToString());
                if(cur.IsOnPath(special) && CastleLevelManager.ObtainedPrereqs(special) && specials[special] != null)
                    specials[special].SetActive(true);
            }
        }

        if(doors != null)
        {
            // disables doors if this is a special room
            doors.SetActive(!cur.IsSpecial());
        }
    }

    public int GetSpecialType(Maze cur)
    {
        for(int special = 0; special < specials.Length; special++)
            if(cur.IsOnPath(special) && CastleLevelManager.ObtainedPrereqs(special) && specials[special] != null)
                return special;
        return -1;
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
        if(rand == null) rand = new System.Random(seed);
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
                // Debug.Log("dir is " + dir + " for path " + special);
                cur = cur.GetNext(dir);
                cur.SetOnPath(special);
            }
        }

        return root;
    }

    public static Maze GetMaze()
    {
        return maze;
    }
    public static Stack<Maze> GetPath()
    {
        return new Stack<Maze>(path);
    }
}
