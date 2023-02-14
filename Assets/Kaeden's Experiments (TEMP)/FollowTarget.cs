using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    private RectTransform myRectTransform;
    public GameObject target;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        myRectTransform = GetComponent<RectTransform>();
    }
    // Update is called once per frame
    void Update()
    {
        myRectTransform.position = new Vector3(target.transform.position.x + offset.x, target.transform.position.y + offset.y, target.transform.position.z + offset.z);
    }
}
