using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    [SerializeField] private Image interactPrompt;
    private bool isOpenable = false;

    public Item loot;

    private void Update()
    {
        if (isOpenable)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Interact();
            }
        }
    }

    private void Interact()
    {
        bool itemAdded = InventoryManager.instance.AddItem(loot);
        if (itemAdded)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpenable = true;
            interactPrompt.gameObject.SetActive(true);
            Camera camera = Camera.main;
            interactPrompt.transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpenable = false;
            interactPrompt.gameObject.SetActive(false);
        }
    }

}
