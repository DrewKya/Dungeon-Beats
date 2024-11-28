using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Item : ScriptableObject
{
    public string itemName = "Item Name";
    public string itemDescription = "New description";
    public int itemPrice = 1;
    public Sprite icon = null;

    public virtual void EquipToPlayer() { }
    public virtual void Drop()
    {
        InventoryManager.instance.RemoveItem(this);
    }
}

[System.Serializable]
public class StatModifiers
{
    public int attackModifier = 0;
    public int healthModifier = 0;
    public int defenseModifier = 0;
    public int critRateModifier = 0;
}
