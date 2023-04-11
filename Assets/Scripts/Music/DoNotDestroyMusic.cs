using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroyMusic : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake() 
    {   
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("LevelTheme");  

        if (musicObj.Length > 1)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject); 
    }
}