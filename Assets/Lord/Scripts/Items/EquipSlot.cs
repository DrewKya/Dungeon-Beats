using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour
{
    public Image icon;

    public Item item;
    private Button button;

    [SerializeField] private Sprite emptyIcon;

    private void Start()
    {
        if(icon == null) icon = GetComponentInChildren<Image>();

        button = GetComponent<Button>();
        button.onClick.AddListener(() => UnequipItem());
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = emptyIcon;
    }

    public void UnequipItem()
    {
        if (item != null)
        {
            bool isUnequipped = false;

            if(item is Equipment)
            {
                Equipment equipment = (Equipment)item;
                isUnequipped = PlayerManager.instance.UnequipItem(equipment);
            }
            else if(item is Weapon)
            {
                Weapon weapon = (Weapon)item;
                isUnequipped = PlayerManager.instance.UnequipItem(weapon);
            }
            else if (item is Consumable)
            {
                Consumable consumable = (Consumable)item;
                isUnequipped = PlayerManager.instance.UnequipItem(consumable);
            }
             
            if (isUnequipped) ClearSlot();
        }
    }
}
