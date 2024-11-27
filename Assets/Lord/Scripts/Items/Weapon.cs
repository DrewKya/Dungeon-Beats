using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item
{
    public GameObject weaponModel;
    public GameObject offhandModel;

    public StatModifiers stats;
    public float attackCooldownInSeconds;

    public enum AnimationType
    {
        None = 0,
        DualSword = 1,
        Greatsword = 2,
        Bow = 3,
        Wand = 4
    };
    public AnimationType animationType;

    public virtual void Initialize(Transform weaponAttachPoint, Transform offhandAttachPoint)
    {
        //Do something in the derived class
    }

    public virtual void Attack()
    {
        //Do something in the derived class
    }

    public override void EquipToPlayer()
    {
        PlayerManager.instance.EquipItem(this);
        InventoryManager.instance.RemoveItem(this);
    }
}
