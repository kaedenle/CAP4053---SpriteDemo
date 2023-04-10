using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainVisual : MonoBehaviour
{
    public int StartPos;
    public int EndPos;
    public float speed;
    public bool TriggerOnStart;
    private bool Triggered;
    // Start is called before the first frame update
    void Start()
    {
        Triggered = TriggerOnStart;
        gameObject.transform.position = new Vector3(StartPos, gameObject.transform.position.y, gameObject.transform.position.z);
    }
    private void PlayTrain()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + speed, gameObject.transform.position.y, gameObject.transform.position.z);
        //end conditions
        if((speed > 0 && gameObject.transform.position.x > EndPos) || (speed < 0 && gameObject.transform.position.x < EndPos))
        {
            StartCoroutine(WaitTillGoAgain());   
        }
    }
    IEnumerator WaitTillGoAgain()
    {
        gameObject.transform.position = new Vector3(StartPos, gameObject.transform.position.y, gameObject.transform.position.z);
        Triggered = false;
        int time = Random.Range(5, 6);
        yield return new WaitForSeconds(time);
        //50% to go the other way
        if (Random.value < 0.5f) Reverse();
        Triggered = true;
    }
    private void Reverse()
    {
        int temp = StartPos;
        StartPos = EndPos;
        EndPos = temp;
        speed *= -1;
    }
    // Update is called once per frame
    void Update()
    {
        if(Triggered) PlayTrain();
    }
}
