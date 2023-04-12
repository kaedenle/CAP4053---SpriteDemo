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

    protected override void ActivateBehaviors()
    {
        base.ActivateBehaviors();
        maze.TakeStairs(direction);
    }
}
