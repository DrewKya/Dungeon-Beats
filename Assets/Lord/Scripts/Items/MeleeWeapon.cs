using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New melee weapon", menuName = "Inventory/Weapon/Melee")]
public class MeleeWeapon : Weapon
{
    public HitboxData hitbox;

    public void Initialize(Transform weaponAttachPoint, Transform offhandAttachPoint)
    {
        if (weaponModel != null)
        {
            Instantiate(weaponModel, weaponAttachPoint);
        }
        if (offhandModel != null)
        {
            Instantiate(offhandModel, offhandAttachPoint);
        }
    }

    public override void Attack()
    {
        Debug.Log("Melee attack!");
    }
}
