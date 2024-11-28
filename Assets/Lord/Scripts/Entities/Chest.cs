using TMPro;
using UnityEngine;

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
        bool itemAdded = InventoryManager.instance.AddItemToInventory(loot);
        if (itemAdded)
        {
            NotificationUI.instance.ItemObtainedNotification(loot);

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
