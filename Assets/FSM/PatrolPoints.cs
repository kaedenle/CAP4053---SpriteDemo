using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoints : MonoBehaviour
{
    public List<Vector3> points;
    private Vector3 targetPoint;
    private int index;
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        targetPoint = points[index];
    }


    public bool HasReachedPoint()
    {
        return (Vector3.Distance(transform.position, targetPoint) <= 0.05f) ? true : false;
    }

    public void SetNextTargetPoint()
    {
        index = (index == points.Count -1) ? 0 : index +1;
        targetPoint = points[index];
    }
    public Vector3 GetTargetPointDirection()
    {
        return (targetPoint - transform.position).normalized;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
