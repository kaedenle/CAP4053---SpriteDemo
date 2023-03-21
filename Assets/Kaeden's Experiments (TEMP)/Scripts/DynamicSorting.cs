using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSorting : MonoBehaviour
{
    private List<Renderer> rends;
    private void GetChildRecursive(GameObject obj)
    {
        if (null == obj)
            return;
        Renderer rend = obj?.GetComponent<Renderer>();
        if (rend) rends.Add(rend);
        foreach (Transform child in obj.transform)
        {
            if (null == child)
                continue;
            GetChildRecursive(child.gameObject);
        }
    }
    private void UpdateRends()
    {
        foreach(Renderer r in rends)
        {
            r.sortingOrder = (int)(r.transform.position.y * -100);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rends = new List<Renderer>();
        GetChildRecursive(gameObject);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UpdateRends();
    }
}
