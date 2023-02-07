using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBoxController : MonoBehaviour
{
    [SerializeField] ScenesManager.AllScenes _outsideBuilding;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if(ScenesManager.GetPreviousScene() == _outsideBuilding)
        {
            player.transform.position = gameObject.transform.position;
        }
    }
}
