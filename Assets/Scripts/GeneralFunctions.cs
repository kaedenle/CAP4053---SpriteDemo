using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFunctions
{
    private static bool DEBUG = true;
    public static bool IsDebug()
    {
        return DEBUG;
    }

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

    // gets an immediate child object with name or returns null
    public static GameObject GetChildByName(GameObject parent, string name)
    {
        if(parent == null) return null;

        foreach(Transform child in parent.transform)
            if(child.gameObject.name.Equals(name))
                return child.gameObject;
        return null;
    }

    public static Camera GetMainCamera()
    {
        return UnityEngine.Camera.main;
    }

    public static float GetCameraHeight()
    {
        return 2f * GetMainCamera().orthographicSize;
    }

    public static float GetCameraWidth()
    {
        return GetCameraHeight() * GetMainCamera().aspect;
    }
}
