using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze
{
    const int specials = 3;
    const int directions = 3;

    Maze[] children;

    // decorations
    bool[] decorations;

    // hints
    bool[] onPath; // on path
    int hints;

    public Maze(int depth_left)
    {
        // Debug.Log("making maze node with depth " + depth_left);
        onPath = new bool[specials];

        if(depth_left == 0) return;

        children = new Maze[directions];

        for(int i = 0; i < directions; i++)
            children[i] = new Maze(depth_left - 1);
    }

    public bool NotSetup()
    {
        return decorations == null;
    }

    public void SetOnPath(int special)
    {
        onPath[special] = true;
    }

    public bool IsOnPath(int special)
    {
        return onPath[special];
    }

    public Maze GetNext(int direction)
    {
        return children[direction];
    }

    public bool IsTerminal()
    {
        return children == null;
    }

    public bool IsSpecial()
    {
        return children == null && (IsOnPath(0) || IsOnPath(1) || IsOnPath(2));
    }

    public bool[] GetDecorations()
    {
        return decorations;
    }

    public void SetDecorations(bool[] decorations)
    {
        this.decorations = decorations;
    }

    public void SetHints(int hint)
    {
        this.hints = hint;
    }

    public int GetHint()
    {
        return hints;
    }
}
