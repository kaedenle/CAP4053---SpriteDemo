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
    public const int directions = 3,
              seed = 42_069_420, 
              pathLenth = 5;  // The length of the maze path (number of rooms traversed until exit) [the lower this goes, the more likely a collision could occur]
   

    private static System.Random rand;
    private static Maze maze;
    private static Stack<int> path;
    private static Stack<Direction> dirPath;

    void Awake()
    {
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
        if(maze == null) maze = GenerateMaze();
        if(rand == null) rand = new System.Random(seed);
        if(path == null) path = new Stack<int>();

        if(path.Count <= 0)
            path.Push(0);    // start in the first room  
        
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

        // Debug.Log("in maze room " + GetCurrentRoom() + " is on path: " + GetCurrentRoom().IsOnPath(0) + " which is terminal: " + GetCurrentRoom().IsTerminal() + " and is special: " + GetCurrentRoom().IsSpecial());
        SetupRoom(GetCurrentRoom());
    }

    public static bool MazeCreated()
    {
        return maze != null;
    }

    public static void SetMaze(Maze m)
    {
        maze = m;
    }

    public static void SetPath(Stack<int> newPath)
    {
        path = new Stack<int> (newPath);
    }

    public Room GetCurrentRoom()
    {
        return maze.maze[path.Peek()];
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
            maze.current = path.Peek();
            RestartRoom();

            return;
        }

        // go to the next room
        Room cur = GetCurrentRoom();

        // is the end room
        if(cur.IsTerminal())
        {
            EndOfPath();
            return;
        }

        path.Push(maze.GetNextIndex((int)dir, cur));
        maze.current = path.Peek();
        dirPath.Push(dir);
        RestartRoom();
    }

    public void RestartRoom()
    {
        ScenesManager.ReloadScene();
    }

    public void SetupRoom(Room cur, bool notsetup=false)
    {
        if(notsetup)
        {
            maze.SetupRoom(cur, rand, decorations.Length, averageNumberOfDecorations / (double) decorations.Length);
            GameData.GetInstance().UpdateMaze(maze);
        }

        // set up the decorations
        bool[] dec_state = cur.GetDecorations();

        for(int i = 0; i < decorations.Length; i++)
        {
            if(decorations[i] == null) continue;

            decorations[i].SetActive(dec_state[i]);
        }

        // by default, turn off specials
        foreach(GameObject obj in specials)
            if(obj != null)
                obj.SetActive(false);

        // set up hints
        hints.SetHints(cur, maze);

        // set up specials
        // (specials default should be false)
        if(cur.IsTerminal())
        {
            // Debug.Log("is terminal; specials.Length=" + specials.Length);
            for(int special = 0; special < specials.Length; special++)
            {
                // Debug.Log("is on path: " + cur.IsOnPath(special).ToString() + " obtained prereqs: " + CastleLevelManager.ObtainedPrereqs(special).ToString() + " is not null: " + (specials[special] != null).ToString());
                if(cur.IsOnPath(special) && CastleLevelManager.ObtainedPrereqs(special) && specials[special] != null)
                {
                    specials[special].SetActive(true);
                    GameData.GetInstance().UpdateMazeIndex(maze.current);
                    GameData.GetInstance().SaveCurrentData();
                }
            }
        }

        if(doors != null)
        {
            // disables doors if this is a special room
            doors.SetActive(!cur.IsSpecial());
        }
    }

    public int GetSpecialType(Room cur)
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
        maze.current = 0;
        ScenesManager.LoadScene(ScenesManager.AllScenes.CastleArena);
    }

    // generates all the paths of the maze
    public Maze GenerateMaze()
    {
        if(rand == null) rand = new System.Random(seed);
        // make trie maze
        maze = new Maze(decorations.Length, averageNumberOfDecorations / (double) decorations.Length, rand);

        // make the special paths
        for(int special = 0; special < specials.Length; special ++)
        {
            Room cur = maze.GetDefault();
            cur.SetOnPath(special);

            while(!cur.IsTerminal())
            {
                int dir = rand.Next(0, directions);
                // Debug.Log("dir is " + dir + " for path " + special);
                cur = maze.GetNext(dir, cur);
                cur.SetOnPath(special);
            }
        }

        for(int i = 0; i < maze.maze.Length; i++)
            // SetupRoom(maze.maze[i], true);
            maze.SetupRoom(maze.maze[i], rand, decorations.Length, averageNumberOfDecorations / (double) decorations.Length);
        
        GameData.GetInstance().UpdateMaze(maze);
        return maze;
    }

    public static Maze GetMaze()
    {
        return maze;
    }
}
