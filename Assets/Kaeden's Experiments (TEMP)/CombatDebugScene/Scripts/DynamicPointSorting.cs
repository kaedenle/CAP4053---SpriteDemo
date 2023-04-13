using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PUT UNDER PARENT OBJECT YOU WANT TO DYNAMICALLY SORT (EVEN IF ITS A GROUP OF OBJECTS)
public class DynamicPointSorting : MonoBehaviour
{
    private List<Renderer> rends;
    public int deltaDown;
    public bool dynamic;
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
        float pos = transform.position.y;
        foreach (Renderer r in rends)
        {
            r.sortingOrder = (int)(pos * -100) + deltaDown;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject parent = gameObject.transform.parent.gameObject;;
        rends = new List<Renderer>();
        //only do it for the parent
        GetChildRecursive(parent);
        UpdateRends();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (dynamic) UpdateRends();
    }

}
