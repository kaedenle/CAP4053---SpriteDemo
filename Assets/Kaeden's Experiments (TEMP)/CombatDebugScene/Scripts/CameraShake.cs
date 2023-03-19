using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPos;
    public IEnumerator Shake(float duration, float magnitude)
    {
        
        originalPos = transform.localPosition;
        float elapsedTime = 0f;
        while(elapsedTime < duration)
        {
            while (EntityManager.IsPaused()) yield return null;
            float xOffset = Random.Range(-0.5f, 0.5f) * magnitude;
            float yOffset = Random.Range(-0.5f, 0.5f) * magnitude;

            transform.localPosition = new Vector3(xOffset, yOffset, originalPos.z);

            elapsedTime += Time.deltaTime;

            //wait one frame
            yield return null;
        }

        transform.localPosition = originalPos;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
