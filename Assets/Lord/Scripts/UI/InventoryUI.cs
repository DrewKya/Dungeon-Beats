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

    public EquipSlot weaponSlot;

    public EquipSlot consumableSlot;

    private void Start()
    {
        playerManager = PlayerManager.instance;

        inventoryManager = InventoryManager.instance;

        slots = itemsGrid.GetComponentsInChildren<InventorySlot>();
        UpdateUI();
    }

    public void UpdateUI()
    {
        if(!inventoryManager || !playerManager)
        {
            return;
        }

        Debug.Log(inventoryManager.items.Count);

        for(int i = 0; i < slots.Length; i++)
        {
            //Debug.Log($"Adding {inventoryManager.items[i].name}");
            if (i < inventoryManager.items.Count && inventoryManager.items[i] != null)
            {
                
                slots[i].AddItemToSlot(inventoryManager.items[i]);
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

        if (playerManager.currentWeapon != null)
        {
            weaponSlot.AddItem(playerManager.currentWeapon);
        }
        else
        {
            weaponSlot.ClearSlot();
        }

        if (playerManager.currentConsumable != null)
        {
            consumableSlot.AddItem(playerManager.currentConsumable);
        }
        else
        {
            consumableSlot.ClearSlot();
        }
    }

    private void OnEnable()
    {
        UpdateUI();
    }
}
