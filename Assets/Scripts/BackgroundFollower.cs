using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script has the attached background object follow the player
// such that the background will pass slower than the player walks
// currently assumes that the background will only follow the player in the x direction
public class BackgroundFollower : MonoBehaviour
{
    // the player game object
    public GameObject player;
    // the percentage of this object that will remain on screen after a change in player position 
    [Range(0, 100)] public float percentFollow;

    // private variables to keep track of position changes
    private float prev_x; 

    // Start is called before the first frame update
    void Start()
    {
        // set the initial player x position
        prev_x = player.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        // calculate the position change and new value
        float delta_x = player.transform.position.x - prev_x;

        if (delta_x == 0) return; // don't want to do any more calculations than we have to

        float new_x = transform.position.x + (delta_x * (percentFollow / 100f));

        // update the position of this object and record the new x
        transform.position = new Vector3(new_x, transform.position.y, transform.position.z);
        prev_x = player.transform.position.x;
    }
}
