using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance {  get; private set; }

    [SerializeField] private GameObject MenuUI;
    [SerializeField] private Camera playerPreviewCamera;
    public bool isPaused { get; private set; }

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
        if (playerPreviewCamera == null) Debug.LogError("Player camera not assigned!");

        MenuUI.SetActive(false);
        playerPreviewCamera.enabled = false;
        isPaused = false;
    }

    public void TogglePauseGame(bool boolean)
    {
        if (boolean == true)
        {
            //Time.timeScale = 1f;
            playerPreviewCamera.enabled = true;
            isPaused = true;
            MenuUI.SetActive(true);
        }
        else
        {
            //Time.timeScale = 1f;
            playerPreviewCamera.enabled = false;
            isPaused = false;
            MenuUI.SetActive(false);
        }
    }

}
