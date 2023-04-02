using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDoor : Interactive
{
    [SerializeField] public MazeManager.Direction direction;
    private MazeManager maze;

    new void Awake()
    {
        base.Awake();
        maze = FindObjectOfType<MazeManager>();
    }

    new void Update()
    {
        base.Update();

        if(ActivateBehavior())
        {
            maze.TakeStairs(direction);
        }
    }
}
