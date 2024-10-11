using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

using MeleeRange = MeleeWeapon.MeleeRange;

public class PlayerEntity : MonoBehaviour, IDamageable
{
    private PlayerManager playerManager;
    private IngameParametersUI parametersUI;

    public PlayerStats stats;

    public int currentHP;

    private Weapon selectedWeapon;
    public GameObject hitboxRangeIndicator;
    public MeleeHitboxTrigger meleeHitbox;
    public Transform weaponAttachPoint;

    public float nextAttackTime; //determines the next Time.time the player can attack
    public bool isCharging; //determines if player is charging an attack

    private void Start()
    {
        InitializeStats();
        parametersUI = IngameParametersUI.instance;
        parametersUI.UpdateHealthPointsUI(currentHP, stats.healthPoint);
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
        stats.critRate = template.critRate;
    }

    public void HoldAttack()
    {
        if (isCharging) return;

        selectedWeapon = playerManager.currentWeapon1;
        if(selectedWeapon == null)
        {
            Debug.Log("No weapon equipped");
            return;
        }

        if (!CheckAttackCooldown())
        {
            Debug.Log("Weapon is in cooldown!");
            return;
        }

        isCharging = true;
        PreviewAttack();
    }

    public void PreviewAttack()
    {
        if(selectedWeapon is MeleeWeapon)
        {
            MeleeWeapon meleeWeapon = (MeleeWeapon)selectedWeapon;
            CheckHitboxRange(meleeWeapon.range);
            hitboxRangeIndicator.SetActive(true);
        }
    }

    public void Attack()
    {
        if(selectedWeapon == null || !isCharging)
        {
            return;
        }

        if (selectedWeapon is MeleeWeapon)
        {
            Debug.Log("Player is attacking!");
            StartCoroutine(ToggleHitbox());
        }

        StartCoroutine(parametersUI.weaponIcon.StartCooldown(selectedWeapon.attackCooldownInSeconds));
        nextAttackTime = Time.time + selectedWeapon.attackCooldownInSeconds;
        hitboxRangeIndicator.SetActive(false);
        isCharging = false;
    }

    private bool CheckAttackCooldown()
    {
        return (Time.time >= nextAttackTime) ? true : false;
    }

    private void CheckHitboxRange(MeleeRange range)
    {
        var collider = meleeHitbox.GetComponent<BoxCollider>();
        var indicator = hitboxRangeIndicator.transform;

        switch (range)
        {
            case MeleeRange.Range1x1:
                SetHitboxSize(new Vector3(0f, 0.5f, 1f), new Vector3(1f, 1f, 1f));
                break;
            case MeleeRange.Range1x2:
                SetHitboxSize(new Vector3(0f, 0.5f, 1.5f), new Vector3(1f, 1f, 2f));
                break;
            default:
                Debug.Log("Cannot find melee weapon range!");
                break;
        }
    }

    private void SetHitboxSize(Vector3 position, Vector3 size)
    {
        var collider = meleeHitbox.GetComponent<BoxCollider>();
        var indicator = hitboxRangeIndicator.transform;

        indicator.localPosition = new Vector3(position.x, 0f, position.z);
        indicator.localScale = size;

        collider.center = position;
        collider.size = new Vector3(size.x * 0.8f, size.y, size.z * 0.8f);
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
        if(critRoll < stats.critRate)
        {
            meleeHitbox.isCrit = true;
            return Mathf.FloorToInt(stats.attack * 1.5f); //crit damage
        }
        else
        {
            meleeHitbox.isCrit = false;
            return stats.attack;
        }
    }

    public void TakeDamage(int damage, bool isCrit = false)
    {
        int totalDamage = CalculateDamageTaken(damage);

        PopupPool.instance.ShowDamage(transform.position, totalDamage, isCrit);
        currentHP -= totalDamage;

        parametersUI.UpdateHealthPointsUI(currentHP, stats.healthPoint);

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
