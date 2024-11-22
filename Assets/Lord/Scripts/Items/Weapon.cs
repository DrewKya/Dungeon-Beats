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
        GreatSword = 2
    };
    public AnimationType animationType;

    public void Initialize(Transform weaponAttachPoint, Transform offhandAttachPoint)
    {
        if (weaponModel != null)
        {
            Instantiate(weaponModel, weaponAttachPoint);
        }
        if(offhandModel != null)
        {
            Instantiate(offhandModel, offhandAttachPoint);
        }
    }

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
