using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpManager : MonoBehaviour
{
    public static int WarpNumber = -1;
    private GameObject player;
    public GameObject[] warps;
    public void SetWarpNum(int num)
    {
        WarpNumber = num;
    }
    void Start()
    {
        player = GameObject.Find("Player");
        if(WarpNumber != -1)
        {
            GameObject go = warps[WarpNumber];
            player.transform.position = go.transform.position;

            WarpTile wt = go.GetComponent<WarpTile>();
            bool flip = false;
            if (wt != null) flip = go.GetComponent<WarpTile>().flip;
            player.GetComponent<Animator>().SetBool("flipped", flip);
        }
    }
}
