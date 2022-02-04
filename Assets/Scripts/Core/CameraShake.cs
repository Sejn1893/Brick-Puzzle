using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float ShakeDuration = 1f;
    public float ShakeMagnitude = 0.5f;

    Vector3 _inintialPos;


    // Start is called before the first frame update
    void Start()
    {
        _inintialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayShake()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsedTime = 0;
        while (elapsedTime < ShakeDuration)
        {
            transform.position = _inintialPos + (Vector3)Random.insideUnitCircle * ShakeMagnitude;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = _inintialPos;
    }
}
