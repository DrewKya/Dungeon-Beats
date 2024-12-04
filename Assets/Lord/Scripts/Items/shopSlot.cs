using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shopSlot : MonoBehaviour
{
    public Image icon;
    public Item item;

    [HideInInspector] public Button button;

    public GameEvent onBuyOrSellItem;

    [SerializeField] private AudioClip transactionSFX;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void AddItemToSlot(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void BuyItem(ref int currentCoins)
    {
        if(item == null) return;

        if(item.itemPrice > currentCoins)
        {
            ShopUI.instance?.SetDialogue($"Sorry, but you are <color=#FFFFA9>{item.itemPrice - currentCoins}</color> coins short on that.");
            return;
        }

        if (InventoryManager.instance.AddItemToInventory(item))
        {
            currentCoins -= item.itemPrice;

            ShopUI.instance?.SetDialogue($"Thank you for your patronage.");
            Debug.Log($"{item.itemName} bought for {item.itemPrice} coins");

            onBuyOrSellItem.TriggerEvent();
            SFXManager.instance.PlaySFX(transactionSFX, transform.position);    
        }
        else
        {
            ShopUI.instance?.SetDialogue($"Hmm..? Looks like your inventory is full.");
        }
    }

    public void SellItem(ref int currentCoins)
    {
        if (item == null) return;

        currentCoins += item.itemPrice;
        item.Drop();

        ShopUI.instance?.SetDialogue($"Heh heh... A pleasure doing business with you.");
        Debug.Log($"{item.itemName} sold for {item.itemPrice} coins");

        onBuyOrSellItem.TriggerEvent();
        SFXManager.instance.PlaySFX(transactionSFX, transform.position);
    }

    public void ClearAllButtonListeners()
    {
        button.onClick.RemoveAllListeners();
    }
}
