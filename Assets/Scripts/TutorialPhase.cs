using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEditor.UI;
using UnityEngine;

public class TutorialPhase : MonoBehaviour
{
    [SerializeField] private GameObject moveBubbles;
    [SerializeField] private GameObject attatchBubbles;
    [SerializeField] private GameObject moveBBB;
    [SerializeField] private GameObject goal;
    [SerializeField] private GameObject goalTextObject;
    [SerializeField] private GameObject blackScreen;

    [SerializeField] private float emphasizedFontSize;

    public static bool playedTutorial = false;

    // Start is called before the first frame update
    void Start()
    {
        blackScreen.SetActive(false);
        moveBubbles.SetActive(false);
        attatchBubbles.SetActive(false);
        goal.SetActive(false);
        moveBBB.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ShowMoveBubbles()
    {
        Time.timeScale = 0f;
        ToggleTutorialObject(moveBubbles);
        yield return new WaitForSecondsRealtime(1f);
        yield return StartCoroutine(WaitForKeyDown());
        ToggleTutorialObject(moveBubbles);
        Time.timeScale = 1f;
    }

    public IEnumerator ShowAttatchBubbles()
    {
        Time.timeScale = 0f;
        ToggleTutorialObject(attatchBubbles);
        yield return new WaitForSecondsRealtime(1f);
        yield return StartCoroutine(WaitForKeyDown());
        ToggleTutorialObject(attatchBubbles);
        Time.timeScale = 1f;
    }

    public IEnumerator ShowMoveBBB()
    {
        Time.timeScale = 0f;
        ToggleTutorialObject(moveBBB);
        yield return new WaitForSecondsRealtime(1f);
        yield return StartCoroutine(WaitForKeyDown());
        ToggleTutorialObject(moveBBB);
        Time.timeScale = 1f;
    }

    //Expand to goal routine
    public IEnumerator ShowGoal()
    {
        Time.timeScale = 0f;
        ToggleTutorialObject(goal);
        StartCoroutine(EmphasizeText(goalTextObject));
        yield return new WaitForSecondsRealtime(1f);
        yield return StartCoroutine(WaitForKeyDown());
        ToggleTutorialObject(goal);
        Time.timeScale = 1f;
    }

    private IEnumerator EmphasizeText(GameObject textObject)
    {
        TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();

        float elapsedTime = 0f;
        float growTime = 0.4f;
        float shrinkTime = 0.4f;

        while (true)
        {
            elapsedTime = 0f;

            // Growing
            while (elapsedTime < growTime)
            {
                elapsedTime += Time.unscaledDeltaTime;

                float newFontSize = Mathf.Lerp(20f, emphasizedFontSize, EaseIn(elapsedTime / growTime));

                text.fontSize = newFontSize;

                yield return null;
            }

            //Shrinking
            elapsedTime = 0f;

            while (elapsedTime < shrinkTime)
            {
                elapsedTime += Time.unscaledDeltaTime;

                float newFontSize = Mathf.Lerp(emphasizedFontSize, 20f, EaseOut(elapsedTime / growTime));

                text.fontSize = newFontSize;

                yield return null;
            }

        }
    }

    private IEnumerator WaitForKeyDown()
    {
        while (!Input.anyKey)
            yield return null;
    }

    private void ToggleTutorialObject(GameObject toToggle)
    {
        blackScreen.SetActive(!blackScreen.activeSelf);
        toToggle.SetActive(!toToggle.activeSelf);
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
