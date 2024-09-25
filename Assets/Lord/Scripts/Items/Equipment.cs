using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EquipmentType
{
    Helmet, Chest, Leg, Boots
};

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentType equipmentType;
    public StatModifiers stats;

    public override void Use()
    {
        PlayerManager.instance.EquipItem(this);
        InventoryManager.instance.RemoveItem(this);
    }

}

[System.Serializable]
public class StatModifiers
{
    public int attackModifier = 0;
    public int healthModifier = 0;
    public int defenseModifier = 0;
}
