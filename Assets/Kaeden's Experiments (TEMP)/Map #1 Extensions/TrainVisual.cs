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
    private float WaitTill;
    // Start is called before the first frame update
    void Start()
    {
        Triggered = TriggerOnStart;
        gameObject.transform.position = new Vector3(StartPos, gameObject.transform.position.y, gameObject.transform.position.z);
        WaitTill = Random.Range(10f, 20f);
    }
    private void PlayTrain()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + speed, gameObject.transform.position.y, gameObject.transform.position.z);
        //end conditions
        if((speed > 0 && gameObject.transform.position.x > EndPos) || (speed < 0 && gameObject.transform.position.x < EndPos))
        {
            WaitTill = Random.Range(10f, 20f);
            Triggered = false;
        }
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
        if (!Triggered)
        {
            WaitTill -= Time.deltaTime;
            if(WaitTill <= 0)
            {
                Triggered = true;
                if (Random.value < 0.5f) Reverse();
                gameObject.transform.position = new Vector3(StartPos, gameObject.transform.position.y, gameObject.transform.position.z);
            }
        }
        if(Triggered && Time.timeScale == 1f) PlayTrain();
    }
}
