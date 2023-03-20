using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public bool followPlayerX, followPlayerY;
    public bool generateBoundary = true;

    private Color defaultBackgroundColor = Color.black;
    private float targetAspect = 16.0f / 9.0f;
    private float xSize, ySize;
    private Camera cam;

    // Start is called before the first frame update
    void Awake()
    {
        // grabs the Main camera. could potentially grab the camera attached to this object
        cam = UnityEngine.Camera.main;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = defaultBackgroundColor;
        
        // force the camera into our desired aspect ratio
        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetAspect;

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {  
            Rect rect = cam.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;
            
            cam.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = cam.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            cam.rect = rect;
        }

        // generate bounding boxes if its checked
        if(generateBoundary)
        {
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
    }

    // Update is called once per frame
    void Update()
    {
        var x = followPlayerX ? player.transform.position.x : transform.position.x;
        var y = followPlayerY ? player.transform.position.y : transform.position.y;

        transform.position = new Vector3(x, y, transform.position.z);
    }
}
