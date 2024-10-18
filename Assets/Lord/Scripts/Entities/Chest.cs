using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text interactPrompt;
    private bool isOpenable = false;

    public Item loot;



    private void Update()
    {
        if (isOpenable)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                animator.SetTrigger("Open");
                Interact();
            }
        }
    }

    private void Interact()
    {
        bool itemAdded = InventoryManager.instance.AddItem(loot);
        if (itemAdded)
        {
            isOpenable = false;
            interactPrompt.gameObject.SetActive(false);

            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider collider in colliders)
            {
                if (collider.isTrigger) collider.enabled = false;
            }
            //Destroy(gameObject);
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
