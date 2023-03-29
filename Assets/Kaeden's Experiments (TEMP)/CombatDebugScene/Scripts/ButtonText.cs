using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonText : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Text[] affect;
    private Color[] originalColor;
    public Color transformColor;
    void Start()
    {
        originalColor = new Color[affect.Length];
        for (int i = 0; i < affect.Length; i++)
            originalColor[i] = affect[i].color;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (Text txt in affect)
            txt.color = transformColor;
    }
    // add callbacks in the inspector like for button
    public void OnPointerUp(PointerEventData eventData)
    {
        for (int i = 0; i < affect.Length; i++)
            affect[i].color = originalColor[i];
    }
}
