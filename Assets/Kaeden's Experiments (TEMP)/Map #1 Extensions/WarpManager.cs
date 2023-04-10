using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpManager : MonoBehaviour
{
    public static int WarpNumber = -1;
    private GameObject player;
    public GameObject[] warps;
    private bool check = false;
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
            
        }
    }
    void Update()
    {
        if (!check)
        {
            if(WarpNumber == -1)
            {
                check = true;
                return;
            }
            GameObject go = warps[WarpNumber];
            check = true;
            WarpTile wt = go.GetComponent<WarpTile>();
            bool flip = false;
            if (wt != null) flip = go.GetComponent<WarpTile>().flip;
            player.GetComponent<Player_Movement>().flipped = flip;
        }
    }
}
