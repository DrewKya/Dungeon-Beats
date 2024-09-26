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

    public Equipment[] currentEquipment;

    public void EquipItem(Equipment equipment)
    {
        Debug.Log($"Equipping {equipment.itemName}.");
        int equipSlot = (int) equipment.equipmentType; //determine which type of equipment it is

        Equipment currentEquippedItem = null;

        if(currentEquipment[equipSlot] != null)
        {
            currentEquippedItem = currentEquipment[equipSlot];
            InventoryManager.instance.AddItem(currentEquippedItem); //add equipped item back to inventory
        }
        currentEquipment[equipSlot] = equipment;
        
    }

    public void UpdatePlayerStats(Equipment previousItem, Equipment newItem)
    {
        if(previousItem != null)
        {
            playerStats.healthPoint -= previousItem.stats.healthModifier;
            playerStats.attack -= previousItem.stats.attackModifier; 
            playerStats.defense -= previousItem.stats.defenseModifier;
        }
        if(newItem != null)
        {
            playerStats.healthPoint += newItem.stats.healthModifier;
            playerStats.attack += newItem.stats.attackModifier;
            playerStats.defense += newItem.stats.defenseModifier;
        }
        PlayerEntity player = FindFirstObjectByType<PlayerEntity>();
        if (player != null)
        {
            player.UpdateStats();
        }
    }

}

[System.Serializable]
public class PlayerStats
{
    public int level = 1;

    //base stat is for unmodified stats (no equipment)
    private int baseHealthPoint = 10;
    private int baseAttack = 1;
    private int baseDefense = 0;

    public int healthPoint = 10;
    public int attack = 1;
    public int defense = 0;
}
