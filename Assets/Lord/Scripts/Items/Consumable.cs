using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    public string effectDescription;

    public override void EquipToPlayer()
    {
        PlayerManager.instance.EquipItem(this);
        InventoryManager.instance.RemoveItem(this);
    }

    public virtual void UseConsumable(PlayerEntity targetPlayer) { }
}
