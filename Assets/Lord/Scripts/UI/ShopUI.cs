using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    #region Singleton
    public static ShopUI instance { get; private set; }

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
    #endregion

    InventoryManager inventoryManager;
    PlayerManager playerManager;

    [SerializeField] private GameObject shopPanel;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text dialogueText;

    private List<Item> currentAvailableItems;


    public Transform playerItemsGrid;
    public Transform shopItemsGrid;

    shopSlot[] playerItemSlots;
    shopSlot[] shopItemSlots;


    private void Start()
    {
        playerManager = PlayerManager.instance;
        inventoryManager = InventoryManager.instance;

        playerItemSlots = playerItemsGrid.GetComponentsInChildren<shopSlot>();
        shopItemSlots = shopItemsGrid.GetComponentsInChildren<shopSlot>();

        shopPanel.SetActive(false);
    }

    public void InitializeShopUI(ref List<Item> availableItems)
    {
        if (!inventoryManager || !playerManager)
        {
            Debug.LogError("Cannot find inventory manager or player manager");
            return;
        }

        SetDialogue("Welcome to this humble merchant's shop. Anything you fancy?");

        shopPanel.SetActive(true);

        currentAvailableItems = availableItems;
        RefreshShopUI();
    }

    public void RefreshShopUI()
    {
        //Setup player items
        for (int i = 0; i < playerItemSlots.Length; i++)
        {
            var slot = playerItemSlots[i];

            slot.ClearAllButtonListeners();

            if (i < inventoryManager.items.Count)
            {
                slot.AddItemToSlot(inventoryManager.items[i]);
                slot.button.onClick.AddListener(() => slot.SellItem(ref playerManager.coin));
            }
            else
            {
                slot.ClearSlot();
            }
        }

        //Setup shop items
        for (int i = 0; i < shopItemSlots.Length; i++)
        {
            var slot = shopItemSlots[i];

            slot.ClearAllButtonListeners();

            if (i < currentAvailableItems.Count)
            {
                slot.AddItemToSlot(currentAvailableItems[i]);
                slot.button.onClick.AddListener(() => slot.BuyItem(ref playerManager.coin));
            }
            else
            {
                slot.ClearSlot();
            }
        }

        coinText.text = $"Coins : {playerManager.coin.ToString()}";
    }

    public void SetDialogue(string text)
    {
        StartCoroutine(TypeText(text));
    }

    private IEnumerator TypeText(string dialogue)
    {
        dialogueText.text = ""; // Clear existing text
        StringBuilder sb = new StringBuilder();
        int i = 0;

        while (i < dialogue.Length)
        {
            // If the current character is a '<', treat it as the start of a tag
            if (dialogue[i] == '<')
            {
                // Find the closing '>' of the tag
                int tagEnd = dialogue.IndexOf('>', i);
                if (tagEnd != -1)
                {
                    sb.Append(dialogue.Substring(i, tagEnd - i + 1));
                    i = tagEnd + 1;
                    continue;
                }
            }

            // Append the next character normally
            sb.Append(dialogue[i]);
            i++;

            dialogueText.text = sb.ToString();

            yield return new WaitForSeconds(0.01f);
        }
    }


}
