using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public enum Person
    {
        Player, 
        Mobster,
        Child,
        Boss,
        Mom
    }

    public static string[] personName =
    {
        "Detective",
        "Antonio",
        "Kaleigh",
        "Thomas",
        "Mother"
    };

    public static string GetName(Person person)
    {
        return personName[(int) person];
    }
}
