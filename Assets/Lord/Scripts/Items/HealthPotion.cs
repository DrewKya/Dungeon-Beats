using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New health potion", menuName = "Inventory/Consumable/Health potion")]
public class HealthPotion : Consumable
{
    public int healingAmount;

    public override void UseConsumable(PlayerEntity targetPlayer) 
    { 
        targetPlayer.Heal(healingAmount);
    }
}
