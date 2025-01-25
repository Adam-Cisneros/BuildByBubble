using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBubbles : MonoBehaviour
{
    [SerializeField] private BubbleController bubbleController;

    //Generate Bubbles
    [SerializeField] int maxGeneratedBubbles;
    [SerializeField] GameObject bubblePrefab;
    [SerializeField] Transform bubbleSpawnPoint;
    [SerializeField] float bubbleSpawnDelay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(bubbleGameLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator bubbleGameLoop()
    {
        Debug.Log("Entered game loop");
        while (BubbleController.numBubbles < maxGeneratedBubbles)
        {
            Debug.Log("Started routine");
            yield return StartCoroutine(createBubble());
            yield return new WaitForSeconds(bubbleSpawnDelay);
        }

    }

    private IEnumerator createBubble()
    {
        Debug.Log("Created bubble");
        GameObject bubble = Instantiate(bubblePrefab, bubbleSpawnPoint.position, Quaternion.identity);
        yield return new WaitUntil(() => bubble.GetComponent<BubbleController>().isStuck);
    }
}
