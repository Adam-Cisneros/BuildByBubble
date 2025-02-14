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
    [SerializeField] private GameObject selectLevelButton;


    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerSpawnPoint;
    private Transform playerSpawnPointTransform;
    [SerializeField] private SpriteRenderer goalSpriteRenderer;
    [SerializeField] private ParticleSystem confettiPS;

    //Tutorial
    private bool isTutorial;
    [SerializeField] private TutorialPhase tutorialPhase;

    private bool isPositionLocked = true;

    // Start is called before the first frame update
    void Start()
    {
        sceneReloads++;
        playerSpawnPointTransform = playerSpawnPoint.GetComponent<Transform>();
        BubbleController.numBubbles = 0;

        isTutorial = (SceneManager.GetActiveScene().name == "Level1") && !TutorialPhase.playedTutorial;

        //Uncomment to test tutorial on any level
        //isTutorial = true;

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
        selectLevelButton.SetActive(false);
        SetGoalAlpha(0.4f);



        //Show bubble amount
        if (sceneReloads == 1)
        {
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(displayBubbleAmount.countBubbles());
        }



        //Bubble Phase
        yield return StartCoroutine(phaseTitle.BlowupText());

        SFXManager.Instance.PlayLoopingMusic("CasualBubbleLoop", 1f);
        yield return new WaitForSeconds(1f);
        //Tutorial indicator
        if (isTutorial)
        {
            yield return StartCoroutine(tutorialPhase.ShowMoveBubbles());
            yield return StartCoroutine(tutorialPhase.ShowAttatchBubbles());
        }
        yield return StartCoroutine(generateBubbles.bubbleGameLoop());
        SFXManager.Instance.StopLoopingMusic();



        //Player Phase
        phaseTitle.phaseTitleText.text = "Run By Bubble";
        yield return new WaitForSeconds(0.8f);
        yield return StartCoroutine(phaseTitle.BlowupText());
        SFXManager.Instance.PlayLoopingMusic("CasualRunLoop", 1f);
        SetGoalAlpha(1f);

        GameObject player = Instantiate(playerPrefab, playerSpawnPointTransform.position, Quaternion.identity);
        //Tutorial indicator
        if (isTutorial)
        {
            yield return StartCoroutine(tutorialPhase.ShowMoveBBB());
            yield return StartCoroutine(tutorialPhase.ShowGoal());
            TutorialPhase.playedTutorial = true;
        }
        playerSpawnPoint.SetActive(false);
        //Lock player in place
        while (isPositionLocked)
        {
            player.transform.position = playerSpawnPointTransform.position;
            player.GetComponent<PlayerController>().resetVelocity();

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
        LoadLevel.UnlockNextLevel();
        //only show next button if not last level
        nextLevelButton.SetActive(!(SceneManager.GetActiveScene().name == nextLevelButton.GetComponent<NextLevel>().lastLevelName));
        selectLevelButton.SetActive(true);
        winTitle.StartShaking();
    }

    public void SetGoalAlpha(float alpha)
    {
        Color color = goalSpriteRenderer.color;

        color.a = Mathf.Clamp01(alpha);

        goalSpriteRenderer.color = color;
    }
}
