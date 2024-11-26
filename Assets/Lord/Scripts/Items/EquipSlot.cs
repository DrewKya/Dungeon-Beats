using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour
{
    public Image icon;

    public Item item;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => UnequipItem());
    }

    public void AddItem(Item newItem)
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

    public void UnequipItem()
    {
        if (item != null)
        {
            if(item is Equipment)
            {
                Equipment equipment = (Equipment)item;
                PlayerManager.instance.UnequipItem(equipment);
            }else if(item is Weapon)
            {
                Weapon weapon = (Weapon)item;
                PlayerManager.instance.UnequipItem(weapon);
            }
            else if (item is Consumable)
            {
                Debug.Log("Implement a code to unequip consumable");
            }
            ClearSlot();
        }
    }
}
