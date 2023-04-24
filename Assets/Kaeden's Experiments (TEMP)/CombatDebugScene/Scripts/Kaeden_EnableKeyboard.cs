using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kaeden_EnableKeyboard : MonoBehaviour
{
    public bool KeyboardShortCuts { get; set; }
    private Toggle tog;
    private void Awake()
    {
        KeyboardShortCuts = WeaponManager.ShortCutCombos;
        tog = GetComponent<Toggle>();
        tog.isOn = KeyboardShortCuts;
    }
    // Update is called once per frame
    void Update()
    {
        if (KeyboardShortCuts != WeaponManager.ShortCutCombos) WeaponManager.ShortCutCombos = KeyboardShortCuts;
    }
}
