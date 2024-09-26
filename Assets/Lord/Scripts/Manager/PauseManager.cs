using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance {  get; private set; }
    public GameObject MenuUI;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"More than one instance of {instance.GetType()} found!");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        MenuUI.SetActive(false);
    }

    public void TogglePauseGame(bool boolean)
    {
        if (boolean)
        {
            Time.timeScale = 0f; //pause
            MenuUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f; //unpause
            MenuUI.SetActive(false);
        }
    }

}
