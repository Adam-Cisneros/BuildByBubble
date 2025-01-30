using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayBubbleAmount : MonoBehaviour
{
    [SerializeField] private GenerateBubbles generateBubbles;

    [SerializeField] private float maxScale;
    [SerializeField] private float originalPositionX;
    [SerializeField] private float originalPositionY;
    [SerializeField] private float adjustedPositionX;
    [SerializeField] private float adjustedPositionY;

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI bubbleAmountText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetBubbleAmountText()
    {
        if (GameLoop.sceneReloads == 1)
        {
            bubbleAmountText.text = "0";
        }
        else
        {

            bubbleAmountText.text = $"{generateBubbles.maxGeneratedBubbles}";
        }
    }

    public IEnumerator countBubbles()
    {

        float elapsedTime = 0f;
        float growTime = 0.2f;  
        float shrinkTime = 0.1f;
        float countTime = 0.3f;

        // Growing
        while (elapsedTime < growTime)
        {
            elapsedTime += Time.deltaTime;

            float scale = Mathf.Lerp(1f, maxScale, EaseIn(elapsedTime / growTime));
            float positionX = Mathf.Lerp(originalPositionX, adjustedPositionX, elapsedTime / growTime);
            float positionY = Mathf.Lerp(originalPositionY, adjustedPositionY, elapsedTime / growTime);

            rectTransform.anchoredPosition = new Vector2(positionX, positionY);
            transform.localScale = new Vector2(scale, scale);

            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        //Increase bubble amount text
        elapsedTime = 0f;

        while (elapsedTime < countTime)
        {
            elapsedTime += Time.deltaTime;

            int amount = (int)Mathf.Lerp(0f, generateBubbles.maxGeneratedBubbles, elapsedTime / countTime);
            bubbleAmountText.text = $"{amount}";

            yield return null;
        }
        SFXManager.Instance.PlaySFX("MaxBubblesSFX");

        yield return new WaitForSeconds(1f);

        //Shrinking
        elapsedTime = 0f;

        while (elapsedTime < shrinkTime)
        {
            elapsedTime += Time.deltaTime;

            float scale = Mathf.Lerp(maxScale, 1f, EaseOut(elapsedTime / shrinkTime)); 
            float positionX = Mathf.Lerp(adjustedPositionX, originalPositionX, elapsedTime / shrinkTime);
            float positionY = Mathf.Lerp(adjustedPositionY, originalPositionY, elapsedTime / shrinkTime);

            rectTransform.anchoredPosition = new Vector2(positionX, positionY);
            transform.localScale = new Vector2(scale, scale);

            yield return null;
        }
    }

    private float EaseOut(float time)
    {
        return Mathf.Pow(time - 1f, 3) + 1f;

    }

    private float EaseIn(float time)
    {
        return Mathf.Pow(time, 3);
    }

}
