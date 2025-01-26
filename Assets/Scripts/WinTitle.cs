using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinTitle : MonoBehaviour
{
    private Vector3 originalPosition;
    //public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;

    [SerializeField] TextMeshProUGUI winText;

    void Start()
    {
        winText.enabled = false;
        originalPosition = transform.localPosition;
    }

    public void StartShaking()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        winText.enabled = true;
        float elapsed = 0f;

        while (true)
        {
            float offsetX = Random.Range(-shakeMagnitude, shakeMagnitude);
            float offsetY = Random.Range(-shakeMagnitude, shakeMagnitude);

            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        //winText.enabled = false;
        //transform.localPosition = originalPosition;

    }
}

