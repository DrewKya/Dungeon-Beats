using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerEntity : MonoBehaviour, IDamageable
{
    private PlayerManager playerManager;
    public PlayerStats stats;

    public int currentHP;

    private void Start()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        playerManager = PlayerManager.instance;
        CopyStatsFromPlayerManager();
        currentHP = stats.healthPoint;
    }

    public void UpdateStats()
    {
        CopyStatsFromPlayerManager();
        currentHP = Mathf.Min(currentHP, stats.healthPoint);
    }

    public void CopyStatsFromPlayerManager()
    {
        PlayerStats template = playerManager.playerStats;

        stats.level = template.level;
        stats.healthPoint = template.healthPoint;
        stats.attack = template.attack;
        stats.defense = template.defense;
    }

    public void TakeDamage(int damage)
    {
        int totalDamage = CalculateDamage(damage);

        Debug.Log($"{gameObject.name} took {totalDamage} damage!");
        currentHP -= totalDamage;
        if(currentHP < 0)
        {
            PlayerDie();
        }
    }

    private int CalculateDamage(int damage)
    {
        return (damage - stats.defense);
    }

    private void PlayerDie()
    {
        Debug.Log("Player died!");
    }
}
