using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeHints : MonoBehaviour
{
    const int num_directions = 3;
    public GameObject[] bannerHint;
    public GameObject[] matLeft;
    public GameObject[] matUp;
    public GameObject[] matRight;
    public GameObject[][] bannerDoorDirections;
    public GameObject[] footprints;

    void Awake()
    {
        bannerDoorDirections = new GameObject[][] {matLeft, matUp, matRight};
    }

    public void SetHints(Maze cur)
    {
        RemoveAll();

        // don't set up hints if you've reached the special end
        if(cur.IsSpecial())
            return;

        SetBanner(cur.GetBanner());
        SetMats(cur.GetMats());
        SetFootprints(cur);
    }

    public void RemoveAll()
    {
        DisableArray(bannerHint);
        
        if(bannerDoorDirections != null)
            foreach(GameObject[] ls in bannerDoorDirections)
                DisableArray(ls);

        DisableArray(footprints);
    }

    public void DisableArray(GameObject[] arr)
    {
        if(arr == null) return;

        foreach(GameObject obj in arr)
            if(obj != null)
                obj.SetActive(false);
    }

    public void SetBanner(int idx)
    {
        if(bannerHint[idx] != null)
            bannerHint[idx].SetActive(true);
    }

    public void SetMats(int[] permutation)
    {
        if(permutation == null) return;

        for(int i = 0; i < num_directions; i++)
        {
            GameObject obj = bannerDoorDirections[i][permutation[i]];
            if(obj != null)
                obj.SetActive(true);
        }
    }

    public void SetFootprints(Maze cur)
    {
        int footprintSpecial = 1;
        
        Debug.Log("SetFootprints()");
        Debug.Log("is on path of special? " + cur.IsOnPath(footprintSpecial));

        if(cur.IsOnPath(footprintSpecial))
        {
            int idx = cur.GetNextOnPath(footprintSpecial);
            Debug.Log("idx direction: " + idx);

            if(footprints[idx] != null)
                footprints[idx].SetActive(true);
        }
    }
}
