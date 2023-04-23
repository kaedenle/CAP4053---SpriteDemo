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
        FinnKoenig,
        AntonioMind,
        PatrickBlanchard,
        NewsAnchor,
        Associate,
        AntonioFinancialAdvisor,
        AntonioFriend,
        AntonioMistress,
        Thomas,
        ThomasBeforeReveal,
        BossThomas
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
        "Conscience",
        "Dog",
        "Kaleigh Blanchard",
        "Antonio Velucci",
        "Finn Koenig",
        "Capo",
        "Patrick Blanchard",
        "News Anchor",
        "Associate",
        "Financial Advisor",
        "Friend",
        "Mistress",
        "Thomas",
        "Voice From Within",
        "A Tormented Soul"
    };

    public static string GetName(Person person)
    {
        return personName[(int) person];
    }
}
