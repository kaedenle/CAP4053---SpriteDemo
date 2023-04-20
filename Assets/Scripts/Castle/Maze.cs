using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Maze
{
    public const int specials = 3;
    const int directions = 3;

    public Room[] maze;
    public int current;

    // make maze
    public Maze(int num_decor, double decor_prob, System.Random rand)
    {
        current = 0;

        int total = 1;
        for(int count = 0; count < MazeManager.pathLenth; count++) 
            total *= directions;
        
        maze = new Room[total];
        for(int i = 0; i < total; i++)  
            maze[i] = new Room();

        int idx = 1;
        for(int i = 0; i < total && idx + 2 < total; i++)
        {
            maze[i].children = new int[3] {idx, idx + 1, idx + 2};
            idx += 3;
        }

        for(int i = 0; i < total; i++)
        {
            // Debug.Log("making maze node with depth " + depth_left);
            maze[i].onPath = new bool[specials];
            SetupRoom(maze[i], rand, num_decor, decor_prob);
        }
    }

    public int GetNextIndex(int direction, Room room)
    {
        return room.children[direction];
    }

    public Room GetNext(int direction)
    {
        return GetNext(direction, maze[current]);
    }

    public Room GetNext(int direction, Room room)
    {
        return maze[room.children[direction]];
    }

    public void SetupRoom(Room room, System.Random rand, int num_decor, double decor_prob)
    {
        // set up decorations
        room.decorations = new bool[num_decor];

        for(int i = 0; i < room.decorations.Length; i++)
            if(rand.NextDouble() < decor_prob)
                room.decorations[i] = true;

        // set up hint type
        room.banner = rand.Next(0, directions);
        room.mats = MakePermutation(room.GetBanner(), 0, rand, room);
    }

    int[] MakePermutation(int goodItem, int special, System.Random rand, Room room)
    {
        if(room.IsSpecial()) return null;

        // get the permutation
        int[] perm = new int[directions];
        for(int i = 0; i < perm.Length; i++)
            perm[i] = i;

        // shuffle the permutation
        Shuffle(perm, rand);
        
        // set the specific permutation if this needs a specific hint somewhere
        if(room.IsOnPath(special))
        {
            int dir = (GetNext(0, room).IsOnPath(special) ? 0 : (GetNext(1, room).IsOnPath(special) ? 1 : 2));
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

    public Room GetDefault()
    {
        return maze[0];
    }

    public int GetNextOnPath(int special)
    {
        return GetNextOnPath(special, GetCurrentRoom());
    }

    public int GetNextOnPath(int special, Room room)
    {
        if(!room.IsOnPath(special) || room.IsTerminal()) return -1;

        for(int i = 0; i < room.children.Length; i++)
            if(GetNext(i).IsOnPath(special))
                return i;
        
        return -1;
    }

    public Room GetCurrentRoom()
    {
        return maze[current];
    }
}