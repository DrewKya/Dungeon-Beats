using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    public string effectDescription;
    public virtual void UseConsumable(PlayerEntity targetPlayer) { }
}
