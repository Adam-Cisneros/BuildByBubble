using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoop : MonoBehaviour
{
    [SerializeField] private PhaseTitle phaseTitle;
    [SerializeField] private GenerateBubbles generateBubbles;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private SpriteRenderer goalSpriteRenderer;
    [SerializeField] private ParticleSystem confettiPS;
    [SerializeField] private WinTitle winTitle;
    [SerializeField] private GameObject nextLevelButton;

    private bool isPositionLocked = true;

    // Start is called before the first frame update
    void Start()
    {
        BubbleController.numBubbles = 0;
        StartCoroutine(MainGameLoop());   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator MainGameLoop()
    {
        nextLevelButton.SetActive(false);
        SetGoalAlpha(0.5f);

        //Bubble Phase
        yield return StartCoroutine(phaseTitle.BlowupText());
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(generateBubbles.bubbleGameLoop());

        //Player Phase
        phaseTitle.phaseTitleText.text = "Run By Bubble";
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(phaseTitle.BlowupText());
        
        SetGoalAlpha(1f);

        GameObject player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);

        while (isPositionLocked)
        {
            player.transform.position = playerSpawnPoint.position;  

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W))
            {
                isPositionLocked = false;  
            }

            yield return null;  
        }
        player.GetComponent<PlayerController>().resetVelocity();

        yield return new WaitUntil(() => player.GetComponent<PlayerController>().goalReached);

        //Player wins
        confettiPS.Play();
        yield return new WaitForSeconds(1f);
        nextLevelButton.SetActive(true);
        winTitle.StartShaking();

    }

    public void SetGoalAlpha(float alpha)
    {
        Color color = goalSpriteRenderer.color;

        color.a = Mathf.Clamp01(alpha);

        goalSpriteRenderer.color = color;
    }
}
