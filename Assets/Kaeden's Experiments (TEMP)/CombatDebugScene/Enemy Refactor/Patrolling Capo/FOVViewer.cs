using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVViewer : MonoBehaviour
{
    private Mesh mesh;
    public Vector3 origin;
    public float fov;
    public float startingAngle;
    public float viewDistance;
    private bool render;
    private MeshRenderer mr;
    public void SeeFOV()
    {
        int rayCount = 20;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        Vector3[] verticies = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[verticies.Length];
        int[] triangles = new int[rayCount * 3];

        verticies[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        var contactFilter = new ContactFilter2D();
        //can only hit on entity layer and action layer
        contactFilter.layerMask =  LayerMask.GetMask("Raycast");
        contactFilter.useLayerMask = true;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;

            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, contactFilter.layerMask);
            if(raycastHit2D.collider == null)
            {
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                vertex = raycastHit2D.point;
            }

            verticies[vertexIndex] = vertex;

            if(i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            vertexIndex++;

            angle -= angleIncrease;
        }

        mesh.vertices = verticies;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
    // Start is called before the first frame update
    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }
    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = (GetAngleFromVectorFloat(aimDirection) + fov/2f);
    }
    public void SetValues(float dist, float angle)
    {
        this.viewDistance = dist;
        this.fov = angle;
    }
    public void AllowRender()
    {
        render = true;
    }
    void Start()
    {
        //Patrol p = GetComponentInParent<Patrol>();
        origin = Vector3.zero;
        mr = GetComponent<MeshRenderer>();
    }
    private void OnEnable()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (render)
        {
            mesh.Clear();
            mesh.RecalculateNormals();
            mr.enabled = true;
            SeeFOV();
            render = false;
        }
        else
        {
            mr.enabled = false;
        }
            
    }
}
