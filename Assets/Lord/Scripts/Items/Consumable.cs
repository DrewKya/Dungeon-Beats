using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Consumable item")]
public class Consumable : Item
{
    public override void Use()
    {
        base.Use();
        Debug.Log($"Using {this.itemName}.");
    }
}
