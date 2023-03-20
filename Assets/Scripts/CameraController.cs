using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public bool followPlayerX, followPlayerY;

    private float xSize, ySize;
    private Camera cam;

    // Start is called before the first frame update
    void Awake()
    {
        cam = UnityEngine.Camera.main;
        ySize = cam.orthographicSize;
        xSize = ySize * cam.aspect;         

        int[] dy = {0, 0, 1, -1};
        int[] dx = {1, -1, 0, 0};

        for(int d = 0; d < 4; d++)
        {
            BoxCollider2D bound = gameObject.AddComponent<BoxCollider2D>();
            
            Vector2 sz;
            if(dx[d] == 0)
                sz = new Vector2(xSize * 2 , 1);
            else
                sz = new Vector2(1, ySize * 2);

            bound.offset = new Vector2(dx[d] * xSize + dx[d] * (sz.x / 2F), dy[d] * ySize + dy[d] * (sz.y / 2F));
            bound.size = sz;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var x = followPlayerX ? player.transform.position.x : transform.position.x;
        var y = followPlayerY ? player.transform.position.y : transform.position.y;

        transform.position = new Vector3(x, y, transform.position.z);
    }
}
