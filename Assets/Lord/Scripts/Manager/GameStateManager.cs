using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance {  get; private set; }

    public enum GameState
    {
        inGame, inMenu, inShop, gameOver
    }
    public GameState currentState;

    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Camera playerPreviewCamera;

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
        playerPreviewCamera.enabled = false;

        SetState(GameState.inGame);
    }

    private void SetState(GameState state)
    {
        var previousState = currentState;
        currentState = state;

        switch (currentState)
        {
            case GameState.inGame:
                EnterInGameState(previousState);
                break;
            case GameState.inMenu:
                EnterInMenuState();
                break;
            case GameState.inShop:
                EnterInShopState();
                break;
            default:
                Debug.LogWarning($"Invalid state : {currentState}");
                break;
        }
    }

    private void EnterInGameState(GameState previousState)
    {
        //Time.timeScale = 1f;

        if(previousState == GameState.inShop)
        {
            ShopUI.instance?.gameObject.SetActive(false);
        }
        else if (previousState == GameState.inMenu)
        {
            menuUI.SetActive(false);
            playerPreviewCamera.enabled = false;
        }
        Debug.Log("Game state set to InGame.");
    }

    private void EnterInMenuState()
    {
        //Time.timeScale = 0f;

        menuUI.SetActive(true);
        playerPreviewCamera.enabled = true;

        Debug.Log("Game state set to InMenu.");
    }

    private void EnterInShopState()
    {
        //Time.timeScale = 0f;

        ShopUI.instance?.gameObject.SetActive(true);

        Debug.Log("Game state set to InShop.");
    }

    public void EnterGameOverState()
    {
        currentState = GameState.gameOver;

        gameOverUI.SetActive(true);
        GameManager.instance.ResetDataOnDeath();
        Debug.Log("Game state set to GameOver.");
    }

    public bool ToggleGameState(GameState targetState)
    {
        if (currentState == GameState.inGame)
        {
            SetState(targetState);
            return true;
        }
        else
        {
            SetState(GameState.inGame);
            Debug.Log("Gamestate is not inGame, returning to inGame");
            return false;
        }
    }
}
