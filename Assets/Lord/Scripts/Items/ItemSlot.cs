using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image icon;
    public Item item;

    [HideInInspector] public Button button;

    public GameEvent onBuyOrSellItem;

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
            NotificationUI.instance?.TextNotification("Not enough coins!");
            return;
        }

        if (InventoryManager.instance.AddItemToInventory(item))
        {
            currentCoins -= item.itemPrice;

            Debug.Log($"{item.itemName} bought for {item.itemPrice} coins");

            onBuyOrSellItem.TriggerEvent();
        }
    }

    public void SellItem(ref int currentCoins)
    {
        if (item == null) return;

        currentCoins += item.itemPrice;
        item.Drop();

        Debug.Log($"{item.itemName} sold for {item.itemPrice} coins");

        onBuyOrSellItem.TriggerEvent();
    }

    public void ClearAllButtonListeners()
    {
        button.onClick.RemoveAllListeners();
    }
}
