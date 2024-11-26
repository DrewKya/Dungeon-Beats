using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ranged weapon", menuName = "Inventory/Weapon/Ranged")]
public class RangedWeapon : Weapon
{
    public GameObject projectilePrefab;
    public float projectileSpeed;

    public override void Initialize(Transform weaponAttachPoint, Transform offhandAttachPoint)
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

}
