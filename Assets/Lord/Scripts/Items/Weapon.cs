using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item
{
    public GameObject weaponModel;

    public StatModifiers stats;
    public float attackCooldownInSeconds;

    public virtual void Attack()
    {
        //Do something in the derived class
    }

    public override void Use()
    {
        PlayerManager.instance.EquipItem(this);
        InventoryManager.instance.RemoveItem(this);
    }
}
