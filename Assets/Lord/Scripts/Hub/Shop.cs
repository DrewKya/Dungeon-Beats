using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private List<Item> availableItems = new List<Item>();

    [SerializeField] private GameObject interactPrompt;
    private bool isInteractable = false;

    private void Update()
    {
        if (isInteractable)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Interact();
            }
        }
    }

    private void Interact()
    {
        if (GameStateManager.instance.ToggleGameState(GameStateManager.GameState.inShop))
        {
            ShopUI.instance.InitializeShopUI(ref availableItems);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactPrompt.SetActive(true);
            isInteractable = true;

            Camera camera = Camera.main;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInteractable = false;
            interactPrompt.gameObject.SetActive(false);
        }
    }


}
