using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance {  get; private set; }

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

    public void TogglePauseGame(bool boolean)
    {
        if (boolean)
        {
            Time.timeScale = 0f; //pause
        }
        else
        {
            Time.timeScale = 1f; //unpause
        }
    }
}
