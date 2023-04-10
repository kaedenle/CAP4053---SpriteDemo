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
        Mom,
        Placeholder,
        Skelly,
        Tutorial,
        Dog,
        KaleighAdult,
        AntonioVelucci,
        FinnKoenig
    }

    public static string[] personName =
    {
        "Detective",
        "Antonio",
        "Kaleigh",
        "Thomas",
        "Mother",
        "Placeholder",
        "Skelly",
        "You",
        "Dog",
        "Kaleigh Blanchard",
        "Antonio Velucci",
        "Finn Koenig"
    };

    public static string GetName(Person person)
    {
        return personName[(int) person];
    }
}
