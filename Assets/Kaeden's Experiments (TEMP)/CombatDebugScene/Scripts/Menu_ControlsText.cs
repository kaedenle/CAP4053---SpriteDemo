using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_ControlsText : MonoBehaviour
{
    public string text;
    public InputManager.Keys[] reference;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Text[] child = GetComponentsInChildren<Text>();

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
