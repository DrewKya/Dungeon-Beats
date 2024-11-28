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
                Debug.Log("Input");
                Interact();
            }
        }
    }

    private void Interact()
    {
        ShopUI.instance.InitializeShopUI(ref availableItems);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactPrompt.SetActive(true);
            isInteractable = true;

            Camera camera = Camera.main;
            interactPrompt.transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
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
