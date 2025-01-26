using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhaseTitle : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI phaseTitleText;
    [SerializeField] float currencyTextMaxSize;
    [SerializeField] float currencyTextMinSize;
    public string titleText = "Build by Bubble";


    // Start is called before the first frame update
    void Start()
    {
        phaseTitleText.enabled = false;
        phaseTitleText.text = titleText;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator BlowupText()
    {
        yield return new WaitForSeconds(1f);
        phaseTitleText.enabled = true;
        yield return new WaitForSeconds(0.2f);
        SFXManager.Instance.PlaySFX("PhaseStartSFX");

        //Make text bigger
        float elapsedTime = 0f;
        float lerpDuration = 0.2f;

        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;

            float lerpedSize = Mathf.Lerp(currencyTextMinSize, currencyTextMaxSize, elapsedTime / lerpDuration);
            phaseTitleText.fontSize = lerpedSize;

            yield return null;
        }


        yield return new WaitForSeconds(0.8f);

        //Make text smaller
        elapsedTime = 0f;
        lerpDuration = 0.1f;

        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;

            float lerpedSize = Mathf.Lerp(currencyTextMaxSize, currencyTextMinSize, elapsedTime / lerpDuration);
            phaseTitleText.fontSize = lerpedSize;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        phaseTitleText.enabled = false;

    }

}
