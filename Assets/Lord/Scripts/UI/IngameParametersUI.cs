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
    public ConsumableIcon consumableIcon;
    public UltimateIcon ultimateIcon;

    [SerializeField] private Image healthPointsBarFill;
    [SerializeField] private TMP_Text healthPointsText;

    [SerializeField] private GameObject CountdownPanel;
    [SerializeField] private TMP_Text countDownText;

    private void Start()
    {
        playerManager = PlayerManager.instance;
        CountdownPanel.SetActive(false);
        UpdateItemUI();
    }


    public void UpdateItemUI()
    {
        weaponIcon.SetWeapon(playerManager.currentWeapon);
        consumableIcon.SetItem(playerManager.currentConsumable);
    }

    public void UpdateHealthPointsUI(int currentHP, int maxHP)
    {
        float percentage = (float)currentHP / (float)maxHP;
        healthPointsBarFill.fillAmount = percentage;
        healthPointsText.text = $"{currentHP} / {maxHP}";
    }

    public void SetUltimateIcon(bool boolean)
    {
        if (boolean)
        {
            ultimateIcon.icon.color = Color.white;
        }
        else
        {
            ultimateIcon.icon.color = Color.grey;
        }
    }

    public void StartCountdown(int duration)
    {
        CountdownPanel.SetActive(true);
        StartCoroutine(CountdownCoroutine(duration));
    }

    private IEnumerator CountdownCoroutine(int duration)
    {
        int remainingTime = duration;

        while(remainingTime > 0)
        {
            countDownText.text = remainingTime.ToString();
            yield return new WaitForSeconds(1f);
            remainingTime--;
        }

        countDownText.text = "0";
    }
}
