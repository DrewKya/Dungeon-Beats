using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerEntity : MonoBehaviour, IDamageable
{
    public int healthPoint;
    public int defense;

    public void TakeDamage(int damage)
    {
        int totalDamage = CalculateDamage(damage);

        Debug.Log($"{gameObject.name} took {totalDamage} damage!");
        healthPoint -= totalDamage;
        if(healthPoint < 0)
        {
            PlayerDie();
        }
    }

    private int CalculateDamage(int damage)
    {
        return (damage - defense);
    }

    private void PlayerDie()
    {
        Debug.Log("Player died!");
    }
}
