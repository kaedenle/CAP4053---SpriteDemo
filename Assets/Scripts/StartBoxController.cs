using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBoxController : MonoBehaviour
{
    [SerializeField] ScenesManager.AllScenes _prevScene;
    private GameObject player;

    // Start is called before the first frame update
    protected void Awake()
    {
        // get the player
        player = GeneralFunctions.GetPlayer();

        if(player == null) 
        {
            Debug.Log("didn't find player for StartBoxController");
            return;
        }

        // Debug.Log("detected previous scene as " + ScenesManager.GetPreviousScene() + " for StartBoxController");
        // move the player to the location of the attached object iff the previous scene was _prevScene
        if(ScenesManager.GetPreviousScene() == _prevScene)
        {
            player.transform.position = gameObject.transform.position;
        }
    }
}
