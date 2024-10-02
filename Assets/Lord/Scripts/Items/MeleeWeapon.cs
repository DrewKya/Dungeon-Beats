using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New melee weapon", menuName = "Inventory/Weapon/Melee")]
public class MeleeWeapon : Weapon
{
    public enum MeleeRange
    {
        Range1x1, Range1x2
    }
    public MeleeRange range;

    private MonoBehaviour monoBehaviour;

    public void Initialize(Transform weaponAttachPoint)
    {
        Instantiate(weaponModel, weaponAttachPoint);
    }

    public override void Attack()
    {
        Debug.Log("Melee attack!");
    }
}
