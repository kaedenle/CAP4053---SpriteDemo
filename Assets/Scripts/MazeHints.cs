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
}
