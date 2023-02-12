using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    public enum Phases
    {
        TheBeginning,
        Mobster,
        Child
    }

    public static Phases currentPhase = Phases.Mobster;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("in Start() of HubManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
