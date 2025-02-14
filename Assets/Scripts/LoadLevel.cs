using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static Dictionary<string, bool> levelsUnlocked = new Dictionary<string, bool>();
    [SerializeField] private string levelNum;
    [SerializeField] private TextMeshProUGUI levelNumText;
    [SerializeField] private GameObject lockedIndicator;


    // Start is called before the first frame update
    void Start()
    {

        levelNumText.text = levelNum;
        if (!levelsUnlocked.ContainsKey($"Level{levelNumText.text}"))
        {
            levelsUnlocked.Add($"Level{levelNumText.text}", false);
        }

        levelsUnlocked["Level1"] = true;
        lockedIndicator.SetActive(!levelsUnlocked[$"Level{levelNum}"]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void LoadNextLevel()
    {
        Match currentLevelNum = Regex.Match(SceneManager.GetActiveScene().name, @"\d+");
        int nextLevelNum = int.Parse(currentLevelNum.Value) + 1;
        SceneManager.LoadScene($"Level{nextLevelNum}");
    }

    public static void UnlockNextLevel()
    {
        //Technically unlocks the level past the last level which doesnt exist but thats fine
        Match currentLevelNum = Regex.Match(SceneManager.GetActiveScene().name, @"\d+");
        int nextLevelNum = int.Parse(currentLevelNum.Value) + 1;
        levelsUnlocked[$"Level{nextLevelNum}"] = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (levelsUnlocked[$"Level{levelNum}"])
        //{
            transform.localScale = new Vector3(1f, 1f, 1f) * 1.05f;
        //}
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void TryLoadLevel()
    {
        if (levelsUnlocked[$"Level{levelNum}"])
        {
            SFXManager.Instance.PlaySFX("NextLevelSFX");
            SceneManager.LoadScene($"Level{levelNum}");
            SFXManager.Instance.StopLoopingMusic();
        }
        else
        {
            SFXManager.Instance.PlaySFX("DirtyBubbleSFX");
        }
    }
}
