using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] string lastLevelName; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1f, 1f, 1f) * 1.05f;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void OnButtonPressed()
    {
        SFXManager.Instance.PlaySFX("NextLevelSFX");
        if (SceneManager.GetActiveScene().name == lastLevelName)
        {
            SFXManager.Instance.StopLoopingMusic();
            LoadNextLevel();
            Time.timeScale = 0f;
        }
        else
        {
            GameLoop.sceneReloads = 0;
            SFXManager.Instance.StopLoopingMusic();
            LoadNextLevel();

        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
