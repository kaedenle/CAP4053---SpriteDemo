using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSorting : MonoBehaviour
{
    private List<Renderer> rends;
    public bool TakeParent;
    public int deltaDown;
    public bool dynamic;
    private Renderer original;
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
        if(original != null) pos = original.transform.position.y;

        foreach(Renderer r in rends)
        {
            float newCoords = TakeParent ? pos : r.transform.position.y;
            r.sortingOrder = (int)(newCoords * -100) + deltaDown;

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        original = gameObject.GetComponent<Renderer>();
        rends = new List<Renderer>();
        GetChildRecursive(gameObject);
        UpdateRends();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(dynamic)
            UpdateRends();
    }
}
