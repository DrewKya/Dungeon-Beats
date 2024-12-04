using System.Collections;
using TMPro;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject chestModel;
    [SerializeField] private GameObject coinModel;
    [SerializeField] private TMP_Text interactPrompt;
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
        if(loot == null)
        {
            loot = MapManager.instance?.lootTable.GetRandomItem();
        }


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

            StartCoroutine(PlayAnimationCoroutine(3f));
            Destroy(gameObject, 3f);
        }
    }

    private IEnumerator PlayAnimationCoroutine(float duration)
    {
        animator.SetTrigger("Open");

        string dissolveParameter = "_Dissolve_amount";

        Material newChestMat = new Material(chestModel.GetComponent<Renderer>().material);
        Material newCoinMat = new Material(coinModel.GetComponent<Renderer>().material);

        chestModel.GetComponent<Renderer>().material = newChestMat;
        coinModel.GetComponent<Renderer>().material = newCoinMat;

        yield return new WaitForSeconds(1f);

        float elapsedTime = 0f;
        float dissolveDuration = duration - 1f;
        while (elapsedTime < dissolveDuration)
        {
            float lerpTime = Mathf.Lerp(0, 1, elapsedTime / dissolveDuration);
            newChestMat.SetFloat(dissolveParameter, lerpTime);
            newCoinMat.SetFloat(dissolveParameter, lerpTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(newChestMat);
        Destroy(newCoinMat);
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
