using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public enum SceneState
    {
        MainMenu, Ingame
    };

    public PlayerManager playerManager;
    public int loadedSlotIndex = -1;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"More than one instance of {instance.GetType()} found!");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log($"Loading scene : {sceneName}.");
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void InitializeNewData()
    {
        playerManager = PlayerManager.instance;
        playerManager.playerName = "Default name";
    }

    public void CreateNewSave(int slot)
    {
        InitializeNewData();
        SaveData(slot);
        Debug.Log("Created save data at slot " + slot);
    }

    public void SaveData()
    {
        playerManager = PlayerManager.instance;
        if(loadedSlotIndex < 0)
        {
            Debug.LogError("Cannot find loaded slot index! Save process cancelled.");
            return;
        }
        SaveSystem.SavePlayer(loadedSlotIndex, playerManager);
    }

    public void SaveData(int slot) //this function is used when a slot needs to be specified (E.g. Creating new save file from empty slot)
    {
        playerManager = PlayerManager.instance;
        SaveSystem.SavePlayer(slot, playerManager);
    }

    public void LoadData(int slot)
    {
        SaveData data = SaveSystem.LoadPlayer(slot);

        if(data == null)
        {
            Debug.Log("Cannot find data! Creating new save file....");
            CreateNewSave(slot);
            loadedSlotIndex = slot;
            return;
        }

        playerManager = PlayerManager.instance;
        playerManager.playerName = data.playerName;

        loadedSlotIndex = slot;
    }

    public void DeleteData(int slot)
    {
        SaveSystem.ResetData(slot);
    }

    public void QuitApplication()
    {
        Debug.Log("Application exited!");
        Application.Quit();
    }
    
}
