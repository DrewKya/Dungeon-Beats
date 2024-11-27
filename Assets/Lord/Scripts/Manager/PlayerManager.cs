using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    public static PlayerManager instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"More than one instance of {instance.GetType()} found!");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public string playerName;
    public PlayerStats playerStats;

    public int coin;

    public Equipment[] currentEquipment;
    public Weapon currentWeapon;
    public Consumable currentConsumable;

    public GameEvent onStatsChanged;
    public GameEvent onInventoryChanged;

    public void EquipItem(Equipment equipment)
    {
        //Debug.Log($"Equipping {equipment.itemName}.");
        int equipSlot = (int) equipment.equipmentType; //determine which type of equipment it is

        Equipment currentEquippedItem = null;

        if(currentEquipment[equipSlot] != null)
        {
            currentEquippedItem = currentEquipment[equipSlot];
            InventoryManager.instance.AddItem(currentEquippedItem); //add equipped item back to inventory
        }
        currentEquipment[equipSlot] = equipment;
        UpdatePlayerStats(currentEquippedItem, equipment);
    }

    public void UnequipItem(Equipment equipment)
    {
        //Debug.Log($"Unequipping {equipment.itemName}.");
        int equipSlot = (int)equipment.equipmentType; //determine which type of equipment it is

        if (currentEquipment[equipSlot] != null)
        {
            var currentEquippedItem = currentEquipment[equipSlot];
            InventoryManager.instance.AddItem(currentEquippedItem); //add equipped item back to inventory
            currentEquipment[equipSlot] = null;
        }
        UpdatePlayerStats(equipment, null);
    }

    public void EquipItem(Weapon weapon)
    {
        //Debug.Log($"Equipping {weapon.itemName}.");
        Weapon currentEquippedWeapon = null;

        if (currentWeapon != null)
        {
            currentEquippedWeapon = currentWeapon;
            InventoryManager.instance.AddItem(currentEquippedWeapon); //add equipped item back to inventory
        }
        currentWeapon = weapon;
        UpdatePlayerStats(currentEquippedWeapon, weapon);
    }

    public void UnequipItem(Weapon weapon)
    {
        //Debug.Log($"Unequipping {weapon.itemName}.");

        if (currentWeapon != null)
        {
            var currentEquippedWeapon = currentWeapon;
            InventoryManager.instance.AddItem(currentWeapon); //add equipped item back to inventory
            currentWeapon = null;
        }
        UpdatePlayerStats(weapon, null);
    }

    public void EquipItem(Consumable consumable)
    {
        //Debug.Log($"Equipping {consumable.itemName}.");

        if(currentConsumable != null)
        {
            InventoryManager.instance.AddItem(currentConsumable); //add equipped consumable back to inventory
        }
        currentConsumable = consumable;

        onStatsChanged.TriggerEvent();
    }

    public void UnequipItem(Consumable consumable)
    {
        //Debug.Log($"Unequipping {consumable.itemName}.");

        if (currentConsumable != null)
        {
            InventoryManager.instance.AddItem(currentConsumable); //add equipped item back to inventory
            currentConsumable = null;
        }

        onStatsChanged.TriggerEvent();
    }

    public void ConsumeItem()
    {
        if(currentConsumable != null)
        {
            currentConsumable = null;
        }

        onStatsChanged.TriggerEvent();
    }

    public void UpdatePlayerStats(Equipment previousItem, Equipment newItem)
    {
        if(previousItem != null)
        {
            playerStats.healthPoint -= previousItem.stats.healthModifier;
            playerStats.attack -= previousItem.stats.attackModifier; 
            playerStats.defense -= previousItem.stats.defenseModifier;
            playerStats.critRate -= previousItem.stats.critRateModifier;
        }
        if(newItem != null)
        {
            playerStats.healthPoint += newItem.stats.healthModifier;
            playerStats.attack += newItem.stats.attackModifier;
            playerStats.defense += newItem.stats.defenseModifier;
            playerStats.critRate += newItem.stats.critRateModifier;
        }
        onStatsChanged.TriggerEvent();
    }

    public void UpdatePlayerStats(Weapon previousItem, Weapon newItem)
    {
        if (previousItem != null)
        {
            playerStats.healthPoint -= previousItem.stats.healthModifier;
            playerStats.attack -= previousItem.stats.attackModifier;
            playerStats.defense -= previousItem.stats.defenseModifier;
        }
        if (newItem != null)
        {
            playerStats.healthPoint += newItem.stats.healthModifier;
            playerStats.attack += newItem.stats.attackModifier;
            playerStats.defense += newItem.stats.defenseModifier;
        }
        onStatsChanged.TriggerEvent();
    }

    public void AddCoin(int amount)
    {
        coin += amount;
    }
}

[System.Serializable]
public class PlayerStats
{
    public int level = 1;

    //base stat is for unmodified stats (no equipment)
    //private int baseHealthPoint = 10;
    //private int baseAttack = 1;
    //private int baseDefense = 0;

    public int healthPoint = 10;
    public int attack = 1;
    public int defense = 0;
    public int critRate = 5;
}
