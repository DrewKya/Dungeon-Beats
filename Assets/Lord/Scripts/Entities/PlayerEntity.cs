using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

using MeleeRange = MeleeWeapon.MeleeRange;

public class PlayerEntity : MonoBehaviour, IDamageable
{
    private PlayerManager playerManager;
    public PlayerStats stats;

    public int currentHP;

    public MeleeHitboxTrigger meleeHitbox;
    public Transform weaponAttachPoint;

    public float nextAttackTime; //this determines the next Time.time the player can attack

    private void Start()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        playerManager = PlayerManager.instance;
        CopyStatsFromPlayerManager();
        currentHP = stats.healthPoint;

        SetWeaponModel();
    }

    public void UpdateStats()
    {
        CopyStatsFromPlayerManager();
        currentHP = Mathf.Min(currentHP, stats.healthPoint);
        
        SetWeaponModel();
    }

    private void SetWeaponModel()
    {
        if(weaponAttachPoint.childCount > 0)
        {
            foreach(Transform child in weaponAttachPoint.transform)
            {
                Destroy(child.gameObject);
            }
        }

        if(playerManager.currentWeapon1 is MeleeWeapon)
        {
            MeleeWeapon meleeWeapon = (MeleeWeapon)playerManager.currentWeapon1;
            meleeWeapon.Initialize(weaponAttachPoint);
        }
    }

    public void CopyStatsFromPlayerManager()
    {
        PlayerStats template = playerManager.playerStats;

        stats.level = template.level;
        stats.healthPoint = template.healthPoint;
        stats.attack = template.attack;
        stats.defense = template.defense;
    }

    public void Attack()
    {
        Weapon weapon = playerManager.currentWeapon1;
        if(weapon == null)
        {
            return;
        }
        if(!CheckAttackCooldown())
        {
            Debug.Log("Weapon is in cooldown!");
            return;
        }

        if (weapon is MeleeWeapon)
        {
            Debug.Log("Player is attacking!");
            MeleeWeapon meleeWeapon = (MeleeWeapon)weapon;
            SetHitboxSize(meleeWeapon.range);
            StartCoroutine(ToggleHitbox());
        }
        StartCoroutine(IngameParametersUI.instance.weaponIcon.StartCooldown(weapon.attackCooldownInSeconds));
        nextAttackTime = Time.time + weapon.attackCooldownInSeconds;
    }

    private bool CheckAttackCooldown()
    {
        return (Time.time >= nextAttackTime) ? true : false;
    }

    private void SetHitboxSize(MeleeRange range)
    {
        var collider = meleeHitbox.GetComponent<BoxCollider>();
        switch (range)
        {
            case MeleeRange.Range1x1:
                collider.center = new Vector3(0f, 0.5f, 1f);
                collider.size = new Vector3(0.8f, 1f, 0.8f);
                break;
            case MeleeRange.Range1x2:
                collider.center = new Vector3(0f, 0.5f, 1.5f);
                collider.size = new Vector3(0.8f, 1f, 1.6f);
                break;
            default:
                Debug.LogWarning("Cannot find melee weapon range!");
                break;
        }
    }

    public IEnumerator ToggleHitbox()
    {
        meleeHitbox.damage = CalculateDamageDealt();
        meleeHitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        meleeHitbox.gameObject.SetActive(false);
    }

    private int CalculateDamageDealt()
    {
        float critRoll = UnityEngine.Random.Range(0f, 100f);
        if(critRoll < 5f) //5% chance to crit
        {
            return Mathf.FloorToInt(stats.attack * 1.5f); //crit damage
        }
        else
        {
            return stats.attack;
        }
    }

    public void TakeDamage(int damage)
    {
        int totalDamage = CalculateDamageTaken(damage);

        PopupPool.instance.ShowDamage(transform.position, totalDamage);
        currentHP -= totalDamage;
        if(currentHP < 0)
        {
            PlayerDie();
        }
    }

    private int CalculateDamageTaken(int damage)
    {
        return Math.Max(0, damage - stats.defense);
    }

    private void PlayerDie()
    {
        Debug.Log("Player died!");
    }
}
