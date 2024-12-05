using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New data", menuName = "Save Data")]
[System.Serializable]
public class DataSO : ScriptableObject
{
    public PlayerStats playerStats;

    public int coin;

    public Equipment currentHelm;
    public Equipment currentChest;
    public Equipment currentLeg;
    public Equipment currentBoots;

    public Weapon currentWeapon;
    public Consumable currentConsumable;

    public List<Item> itemsInInventory;
}
