using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCursorOnEnable : MonoBehaviour
{
    void OnEnable()
    {
        Cursor.visible = true;
    }
    void OnDisable()
    {
        Cursor.visible = false;
    }
}