using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    InventoryManager inventoryManager;
    PlayerManager playerManager;

    public Transform itemsGrid;
    InventorySlot[] slots;

    public EquipSlot helmetSlot;
    public EquipSlot chestplateSlot;
    public EquipSlot leggingsSlot;
    public EquipSlot bootsSlot;

    public EquipSlot weaponSlot_1;
    public EquipSlot weaponSlot_2;

    public EquipSlot consumableSlot_1;
    public EquipSlot consumableSlot_2;


    private void Start()
    {
        playerManager = PlayerManager.instance;

        inventoryManager = InventoryManager.instance;
        inventoryManager.OnItemChangedCallback += UpdateUI;

        slots = itemsGrid.GetComponentsInChildren<InventorySlot>();
        UpdateUI();
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

        // not optimized, try to find a better way to update
        for(int i = 0; i<playerManager.currentEquipment.Length; i++)
        {
            var item = playerManager.currentEquipment[i];
            if (item != null)
            {
                switch (item.equipmentType)
                {
                    case EquipmentType.Helmet:
                        helmetSlot.AddItem(item);
                        break;
                    case EquipmentType.Chest:
                        chestplateSlot.AddItem(item);
                        break;
                    case EquipmentType.Leg:
                        leggingsSlot.AddItem(item);
                        break;
                    case EquipmentType.Boots:
                        bootsSlot.AddItem(item);
                        break;
                    default:
                        break;
                }
            }
        }

        if (playerManager.currentWeapon1 != null)
        {
            weaponSlot_1.AddItem(playerManager.currentWeapon1);
        }
    }
}
