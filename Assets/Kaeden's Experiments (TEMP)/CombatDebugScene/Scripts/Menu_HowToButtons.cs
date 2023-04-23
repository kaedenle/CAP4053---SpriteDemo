using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_HowToButtons : MonoBehaviour
{
    public GameObject collection;

    public void show(int i)
    {
        int count = 0;
        foreach(Transform child in collection.transform)
        {
            if (count == i) child.gameObject.SetActive(true);
            else child.gameObject.SetActive(false);
            count++;
        }
    }
}
