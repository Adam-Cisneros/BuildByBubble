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
    [SerializeField] GameObject dropper;

    //Text UI
    [SerializeField] TextMeshProUGUI bubblesLeftText;

    //Indicate Done
    static public bool bubblePhaseOver;

    // Start is called before the first frame update
    void Start()
    {
        bubblePhaseOver = false;
        bubblesLeftText.text = $"{maxGeneratedBubbles}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator bubbleGameLoop()
    {

        while (BubbleController.numBubbles < maxGeneratedBubbles)
        {
            yield return StartCoroutine(createBubble());

        }
        bubblePhaseOver = true;
    }

    private IEnumerator createBubble()
    {
        dropper.GetComponent<Animator>().Play("BBBDropperAni", -1, 0f);
        yield return new WaitForSeconds(bubbleSpawnDelay);
        GameObject bubble = Instantiate(bubblePrefab, bubbleSpawnPoint.position - new Vector3(0f, 0.5f, 0f), Quaternion.identity);
        yield return new WaitUntil(() => bubble.GetComponent<BubbleController>().isStuck);
        bubblesLeftText.text = $"{maxGeneratedBubbles - BubbleController.numBubbles}";
    }
}
