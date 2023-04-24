using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kaeden_EnableKeyboard : MonoBehaviour
{
    public bool KeyboardShortCuts { get; set; }
    // Update is called once per frame
    void Update()
    {
        if (KeyboardShortCuts != WeaponManager.ShortCutCombos) WeaponManager.ShortCutCombos = KeyboardShortCuts;
    }
}
