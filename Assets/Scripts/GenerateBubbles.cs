using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerateBubbles : MonoBehaviour
{
    [SerializeField] private BubbleController bubbleController;

    //Generate Bubbles
    [SerializeField] int maxGeneratedBubbles;
    [SerializeField] GameObject bubblePrefab;
    [SerializeField] Transform bubbleSpawnPoint;
    [SerializeField] float bubbleSpawnDelay;

    //Text UI
    [SerializeField] TextMeshProUGUI bubblesLeftText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(bubbleGameLoop());
        bubblesLeftText.text = $"{maxGeneratedBubbles}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator bubbleGameLoop()
    {
        yield return new WaitForSeconds(1f);
        while (BubbleController.numBubbles < maxGeneratedBubbles)
        {
            yield return StartCoroutine(createBubble());
            yield return new WaitForSeconds(bubbleSpawnDelay);
        }

    }

    private IEnumerator createBubble()
    {
        GameObject bubble = Instantiate(bubblePrefab, bubbleSpawnPoint.position, Quaternion.identity);
        yield return new WaitUntil(() => bubble.GetComponent<BubbleController>().isStuck);
        bubblesLeftText.text = $"{maxGeneratedBubbles - BubbleController.numBubbles}";
    }
}
