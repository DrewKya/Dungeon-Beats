using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    RectTransform rectTransform;

    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemType;
    [SerializeField] private TMP_Text itemStats;
    [SerializeField] private TMP_Text itemDescription;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        SetTooltipPosition();
    }

    private void SetTooltipPosition()
    {
        Vector2 position = Input.mousePosition;

        float pivotX, pivotY;
        pivotX = (position.x > Screen.width / 2) ? 1 : 0;
        pivotY = (position.y > Screen.height / 2) ? 1 : 0;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }

    public void SetContent(Item item)
    {
        itemName.text = item.name;

        if(item is Equipment)
        {
            Equipment equipment = (Equipment)item;

            itemType.text = equipment.equipmentType.ToString();

            itemStats.text = GenerateItemStatsAsString(equipment.stats);
        }
        else if(item is Consumable)
        {
            Consumable consumable = (Consumable)item;

            itemType.text = "Consumable item";
            itemStats.text = "";
        }else if(item is Weapon)
        {
            Weapon weapon = (Weapon)item;

            itemType.text = "Weapon";
            itemStats.text = GenerateItemStatsAsString(weapon.stats);

        }

        itemDescription.text = item.itemDescription;
    }

    private string GenerateItemStatsAsString(StatModifiers stats)
    {
        string result = "";
        if (stats.attackModifier != 0) result += $"Attack : {stats.attackModifier}\n";
        if (stats.healthModifier != 0) result += $"Health : {stats.healthModifier}\n";
        if (stats.defenseModifier != 0) result += $"Defense : {stats.defenseModifier}\n";

        return result;

    }
}
