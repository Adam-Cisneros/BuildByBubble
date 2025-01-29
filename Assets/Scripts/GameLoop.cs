using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoop : MonoBehaviour
{
    public static int sceneReloads = 0;

    //Scripts
    [SerializeField] private GenerateBubbles generateBubbles;
    [SerializeField] private DisplayBubbleAmount displayBubbleAmount;


    //UI
    [SerializeField] private PhaseTitle phaseTitle;
    [SerializeField] private WinTitle winTitle;
    [SerializeField] private GameObject nextLevelButton;


    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerSpawnPoint;
    private Transform playerSpawnPointTransform;
    [SerializeField] private SpriteRenderer goalSpriteRenderer;
    [SerializeField] private ParticleSystem confettiPS;

    private bool isPositionLocked = true;

    // Start is called before the first frame update
    void Start()
    {
        sceneReloads++;
        playerSpawnPointTransform = playerSpawnPoint.GetComponent<Transform>();
        BubbleController.numBubbles = 0;
        StartCoroutine(MainGameLoop());   
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SFXManager.Instance.StopLoopingMusic();
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }


    private IEnumerator MainGameLoop()
    {
        //Init Level
        displayBubbleAmount.resetBubbleAmountText();
        playerSpawnPoint.SetActive(true);
        nextLevelButton.SetActive(false);
        SetGoalAlpha(0.4f);

        if (sceneReloads == 1)
        {
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(displayBubbleAmount.countBubbles());
            yield return new WaitForSeconds(0.5f);
        }

        //Bubble Phase
        yield return StartCoroutine(phaseTitle.BlowupText());

        SFXManager.Instance.PlayLoopingMusic("CasualBubbleLoop", 1f);
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(generateBubbles.bubbleGameLoop());
        SFXManager.Instance.StopLoopingMusic();

        //Player Phase
        phaseTitle.phaseTitleText.text = "Run By Bubble";
        yield return new WaitForSeconds(0.8f);
        yield return StartCoroutine(phaseTitle.BlowupText());
        SFXManager.Instance.PlayLoopingMusic("CasualRunLoop", 1f);

        SetGoalAlpha(1f);

        GameObject player = Instantiate(playerPrefab, playerSpawnPointTransform.position, Quaternion.identity);
        playerSpawnPoint.SetActive(false);

        while (isPositionLocked)
        {
            player.transform.position = playerSpawnPointTransform.position;  

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            {
                isPositionLocked = false;  
            }

            yield return null;  
        }
        player.GetComponent<PlayerController>().resetVelocity();

        yield return new WaitUntil(() => player.GetComponent<PlayerController>().goalReached);
        SFXManager.Instance.StopLoopingMusic();

        //Player wins
        confettiPS.Play();
        yield return new WaitForSeconds(1f);
        SFXManager.Instance.PlayLoopingMusic("BubbleBubbleBubbleLoop", 1f);
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
