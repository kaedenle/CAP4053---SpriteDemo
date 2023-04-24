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
    private AudioSource src;
    private IEnumerator running;
    private Kaeden_VolumeReciever reciever;
    // Start is called before the first frame update
    void Start()
    {
        Triggered = TriggerOnStart;
        gameObject.transform.position = new Vector3(StartPos, gameObject.transform.position.y, gameObject.transform.position.z);
        WaitTill = Random.Range(10f, 20f);
        src = GetComponent<AudioSource>();
        reciever = GetComponent<Kaeden_VolumeReciever>();
    }
    private void PlayTrain()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + speed, gameObject.transform.position.y, gameObject.transform.position.z);
        //end conditions
        if (!src.isPlaying) src.Play();
        if ((speed > 0 && gameObject.transform.position.x > EndPos) || (speed < 0 && gameObject.transform.position.x < EndPos))
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
    public IEnumerator StartFade(float duration, float targetVolume)
    {
        if (src == null) yield break;
        float currentTime = 0;
        float start = src.volume;
        float decreaseTime = 0.01f;
        while (currentTime < duration)
        {
            currentTime += decreaseTime;
            src.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        src.Stop();
        reciever.giveUpControl = false;
        src.volume = Kaeden_VolumeReciever.SFXVolume;
        running = null;
        yield break;
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
                if(running != null) StopCoroutine(running);
                running = null;
                reciever.giveUpControl = false;
                src.Play(); 
            }
            if (running == null && !reciever.giveUpControl)
            {
                running = StartFade(10, 0);
                reciever.giveUpControl = true;
                StartCoroutine(running);
            }
        }
        if(Triggered && Time.timeScale == 1f) PlayTrain();
    }
}
