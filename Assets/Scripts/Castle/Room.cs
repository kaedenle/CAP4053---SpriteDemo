using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable] 
public class Room
{
    public int[] children;
    // decorations
    public bool[] decorations;
    // hints
    public bool[] onPath; // on path
    public int banner;
    public int[] mats;

    public Room()
    {
        onPath = new bool[Maze.specials];
    }

    public bool[] GetDecorations()
    {
        return decorations;
    }

    public int[] GetMats()
    {
        return mats;
    }

    public void SetDecorations(bool[] decorations)
    {
        this.decorations = decorations;
    }

    public int GetBanner()
    {
        return banner;
    }

    public bool IsTerminal()
    {
        return children == null;
    }

    public bool IsSpecial()
    {
        return IsTerminal() && (IsOnPath(0) || IsOnPath(1) || IsOnPath(2));
    }

    public bool IsOnPath(int special)
    {
        return onPath[special];
    }

    public void SetOnPath(int special)
    {
        onPath[special] = true;
    }   
}