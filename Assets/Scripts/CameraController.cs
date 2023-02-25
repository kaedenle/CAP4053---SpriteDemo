using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public bool followPlayerX, followPlayerY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var x = followPlayerX ? player.transform.position.x : transform.position.x;
        var y = followPlayerY ? player.transform.position.y : transform.position.y;

        transform.position = new Vector3(x, y, transform.position.z);
    }
}
