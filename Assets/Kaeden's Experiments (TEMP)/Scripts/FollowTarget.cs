using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    private RectTransform myRectTransform;
    public GameObject target;
    public bool FollowX;
    public bool FollowY;
    public bool AutoScale;
    public Vector3 offset;
    // Start is called before the first frame update
    void Awake()
    {
        myRectTransform = GetComponent<RectTransform>();
        if(AutoScale) myRectTransform.localScale = new Vector3(1, 1, 1);
    }
    // Update is called once per frame
    void LateUpdate()
    {
        float UseX = gameObject.transform.position.x;
        if (FollowX) UseX = target.transform.position.x + offset.x;

        float UseY = gameObject.transform.position.y;
        if (FollowY) UseY = target.transform.position.y + offset.y;

        myRectTransform.position = new Vector3(UseX, UseY, target.transform.position.z + offset.z);
    }
}
