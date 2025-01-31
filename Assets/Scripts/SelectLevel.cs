using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
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

    public void OpenLevelSelect()
    {
        SFXManager.Instance.PlaySFX("NextLevelSFX");
        SFXManager.Instance.StopLoopingMusic();
        SceneManager.LoadScene("LevelSelect");
    }
}
