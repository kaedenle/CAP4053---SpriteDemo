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
    }

    public void RemoveAll()
    {
        foreach(GameObject hint in bannerHint)
            if(hint != null)
                hint.SetActive(false);
        
        if(bannerDoorDirections != null)
            foreach(GameObject[] ls in bannerDoorDirections)
                foreach(GameObject hint in ls)
                    hint.SetActive(false);
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
}
