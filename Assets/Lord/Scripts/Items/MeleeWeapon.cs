using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "New melee weapon", menuName = "Inventory/Weapon/Melee")]
public class MeleeWeapon : Weapon
{
    public HitboxData hitbox;
    public GameObject attackVFX;

    public void Initialize(Transform weaponAttachPoint, Transform offhandAttachPoint, Transform VFXattachPoint)
    {
        if (weaponModel != null)
        {
            Instantiate(weaponModel, weaponAttachPoint);
        }
        if (offhandModel != null)
        {
            Instantiate(offhandModel, offhandAttachPoint);
        }
        if(attackVFX != null && VFXattachPoint != null)
        {
            Instantiate(attackVFX, VFXattachPoint);
        }
    }

    public override void Attack()
    {
        Debug.Log("Melee attack!");
    }
}
