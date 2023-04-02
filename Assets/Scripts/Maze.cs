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
    int banner;
    int[] mats;

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

    public void SetupRoom(System.Random rand, int num_decor, double decor_prob)
    {
        // set up decorations
        decorations = new bool[num_decor];

        for(int i = 0; i < decorations.Length; i++)
            if(rand.NextDouble() < decor_prob)
                decorations[i] = true;

        // set up hint type
        banner = rand.Next(0, directions);
        mats = MakePermutation(GetBanner(), 0, rand);
    }

    int[] MakePermutation(int goodItem, int special, System.Random rand)
    {
        if(IsSpecial()) return null;

        // get the permutation
        int[] perm = new int[directions];
        for(int i = 0; i < perm.Length; i++)
            perm[i] = i;

        // shuffle the permutation
        Shuffle(perm, rand);
        
        // set the specific permutation if this needs a specific hint somewhere
        if(IsOnPath(special))
        {
            int dir = (children[0].IsOnPath(special) ? 0 : (children[1].IsOnPath(special) ? 1 : 2));
            // get the correct
            for(int i = 0; i < perm.Length; i++)
                if(perm[i] == goodItem)
                {
                    swap(i, dir, perm);
                }
        }

        return perm;
    }

    /// Knuth shuffle
    public void Shuffle(int[] array, System.Random rand)
    {
        int n = array.Length - 1;
        while (n > 1)
        {
            n--;
            int i = rand.Next(n + 1);
            swap(i, n, array);
        }
    }

    // swap two values in an array
    public void swap(int i, int j, int[] array)
    {
        int temp = array[i];
        array[i] = array[j];
        array[j] = temp;
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
}
