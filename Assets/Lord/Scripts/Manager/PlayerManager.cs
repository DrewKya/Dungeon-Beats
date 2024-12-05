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

    public void EquipItem(Item item)
    {
        if(item is Equipment)
        {
            var equipment = (Equipment)item;
            EquipItem(equipment);
        }
        else if (item is Weapon)
        {
            var weapon = (Weapon)item;
            EquipItem(weapon);
        }
        else if (item is Consumable)
        {
            var consumable = (Consumable)item;
            EquipItem(consumable);
        }
        onInventoryChanged.TriggerEvent();
    }

    public void EquipItem(Equipment equipment)
    {
        Debug.Log($"Equipping {equipment.itemName}.");
        int equipSlot = (int) equipment.equipmentType; //determine which type of equipment it is

        Equipment currentEquippedItem = null;

        if(currentEquipment[equipSlot] != null)
        {
            currentEquippedItem = currentEquipment[equipSlot];
            InventoryManager.instance.AddItemToInventory(currentEquippedItem); //add equipped item back to inventory
        }
        currentEquipment[equipSlot] = equipment;
        UpdatePlayerStats(currentEquippedItem, equipment);
    }

    public bool UnequipItem(Equipment equipment)
    {
        //Debug.Log($"Unequipping {equipment.itemName}.");
        int equipSlot = (int)equipment.equipmentType; //determine which type of equipment it is

        if (currentEquipment[equipSlot] != null)
        {
            var currentEquippedItem = currentEquipment[equipSlot];
            bool itemAdded = InventoryManager.instance.AddItemToInventory(currentEquippedItem); //add equipped item back to inventory
            if (itemAdded)
            {
                currentEquipment[equipSlot] = null;
                UpdatePlayerStats(equipment, null);
                return true;
            }
        }
        return false; 
    }

    public void EquipItem(Weapon weapon)
    {
        //Debug.Log($"Equipping {weapon.itemName}.");
        Weapon currentEquippedWeapon = null;

        if (currentWeapon != null)
        {
            currentEquippedWeapon = currentWeapon;
            InventoryManager.instance.AddItemToInventory(currentEquippedWeapon); //add equipped item back to inventory
        }
        currentWeapon = weapon;
        UpdatePlayerStats(currentEquippedWeapon, weapon);
    }

    public bool UnequipItem(Weapon weapon)
    {
        //Debug.Log($"Unequipping {weapon.itemName}.");

        if (currentWeapon != null)
        {
            bool itemAdded = InventoryManager.instance.AddItemToInventory(currentWeapon); //add equipped item back to inventory
            if (itemAdded)
            {
                currentWeapon = null;
                UpdatePlayerStats(weapon, null);
                return true;
            }
        }
        return false;
    }

    public void EquipItem(Consumable consumable)
    {
        //Debug.Log($"Equipping {consumable.itemName}.");

        if(currentConsumable != null)
        {
            InventoryManager.instance.AddItemToInventory(currentConsumable); //add equipped consumable back to inventory
        }
        currentConsumable = consumable;

        onStatsChanged.TriggerEvent();
    }

    public bool UnequipItem(Consumable consumable)
    {
        //Debug.Log($"Unequipping {consumable.itemName}.");

        if (currentConsumable != null)
        {
            bool itemAdded = InventoryManager.instance.AddItemToInventory(currentConsumable); //add equipped item back to inventory
            if (itemAdded)
            {
                currentConsumable = null;
                onStatsChanged.TriggerEvent();
                return true;
            } 
        }
        return false;
    }

    public void ConsumeItem()
    {
        if(currentConsumable != null)
        {
            //try to find the same item in inventory
            Consumable nextItem = InventoryManager.instance.items.Find(item => item == currentConsumable) as Consumable;

            if (nextItem != null)
            {
                currentConsumable = nextItem;
                InventoryManager.instance.RemoveItem(nextItem);
            }
            else
            {
                currentConsumable = null;
            }
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

    public PlayerStats(PlayerStats newStats) //use this to copy playerStats
    {
        level = newStats.level;
        healthPoint = newStats.healthPoint;
        attack = newStats.attack;
        defense = newStats.defense;
        critRate = newStats.critRate;
    }
}
