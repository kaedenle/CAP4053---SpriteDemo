using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFunctions
{
    public static GameObject GetPlayer()
    {
        string target = "Player";
        GameObject player = GameObject.FindGameObjectWithTag(target);

        // no player in scene
        if(player == null) return null;

        while(player.transform.parent != null)
        {
            if(player.transform.parent.tag == target)
            {
                player = player.transform.parent.gameObject;
            }
        }

        return player;
    }
}
