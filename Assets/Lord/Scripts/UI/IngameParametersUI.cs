using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

//This script manages parameters UI on a fulscreen canvas, such as health bar, attack cooldowns, etc
public class IngameParametersUI : MonoBehaviour
{
    public static IngameParametersUI instance {private set; get; }

    private void Awake()
    {
        instance = this;
    }

    InventoryManager inventoryManager;
    PlayerManager playerManager;

    public WeaponIcon weaponIcon;
    public Image healthPointsBarFill;

    private void Start()
    {
        playerManager = PlayerManager.instance;
        UpdateItemUI();
    }


    public void UpdateItemUI()
    {
        if (playerManager.currentWeapon1 == null)
        {
            weaponIcon.SetWeapon(null);
            
            return;
        }
        weaponIcon.SetWeapon(playerManager.currentWeapon1);
    }

    public void UpdateHealthPointsUI(int currentHP, int maxHP)
    {
        float percentage = (float)currentHP / (float)maxHP;
        healthPointsBarFill.fillAmount = percentage;
    }
}
