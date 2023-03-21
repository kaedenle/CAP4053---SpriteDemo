using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSorting : MonoBehaviour
{
    private List<Renderer> rends;
    public bool TakeParent;
    public int deltaDown;
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
        float original = gameObject.GetComponent<Renderer>().transform.position.y;
        foreach(Renderer r in rends)
        {
            float newCoords = TakeParent ? original : r.transform.position.y;
            r.sortingOrder = (int)(newCoords * -100) + deltaDown;

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
