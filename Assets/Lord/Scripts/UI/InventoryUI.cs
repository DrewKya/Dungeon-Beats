using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    InventoryManager inventoryManager;

    public Transform itemsGrid;
    InventorySlot[] slots;

    private void Start()
    {
        inventoryManager = InventoryManager.instance;

        slots = itemsGrid.GetComponentsInChildren<InventorySlot>();
    }

    private void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (i < inventoryManager.items.Count)
            {
                slots[i].AddItem(inventoryManager.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
