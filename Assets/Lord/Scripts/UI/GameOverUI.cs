using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] Button backButton;

    private void Start()
    {
        backButton.onClick.AddListener(() => onBackButtonClicked());
    }

    private void onBackButtonClicked()
    {
        if (SceneLoader.instance != null)
            SceneLoader.instance.LoadScene("Hub");
        else
            SceneManager.LoadScene("Hub");
    }
}
