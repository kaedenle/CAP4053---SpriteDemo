using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze
{
    const int specials = 3;
    const int directions = 3;

    Maze[] children;
    bool[] onPath;
    bool[] decorations;

    public Maze(int depth_left)
    {
        // Debug.Log("making maze node with depth " + depth_left);
        onPath = new bool[specials];

        if(depth_left == 0) return;

        children = new Maze[directions];

        for(int i = 0; i < directions; i++)
            children[i] = new Maze(depth_left - 1);
    }

    public void SetOnPath(int special)
    {
        onPath[special] = true;
    }

    public Maze GetNext(int direction)
    {
        return children[direction];
    }

    public bool IsTerminal()
    {
        return children == null;
    }

    public bool[] GetDecorations()
    {
        return decorations;
    }

    public void SetDecorations(bool[] decorations)
    {
        this.decorations = decorations;
    }
}
