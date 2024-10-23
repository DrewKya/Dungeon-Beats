using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerEntity : MonoBehaviour, IDamageable
{
    private PlayerManager playerManager;
    private IngameParametersUI parametersUI;

    public PlayerStats stats;

    public int currentHP;

    [SerializeField] private Animator animator;
    [SerializeField] private PlayableDirector ultDirector;
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
            CheckHitboxRange(meleeWeapon.hitbox);
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
        animator.SetTrigger("Attack");
        StartCoroutine(parametersUI.weaponIcon.StartCooldown(selectedWeapon.attackCooldownInSeconds));
        nextAttackTime = Time.time + selectedWeapon.attackCooldownInSeconds;
        hitboxRangeIndicator.SetActive(false);
        isCharging = false;
    }

    public void TestUltimate() //this is only for testing player ult
    {
        Debug.Log("test");
        ultDirector.Play();
    }

    private bool CheckAttackCooldown()
    {
        return (Time.time >= nextAttackTime) ? true : false;
    }

    private void CheckHitboxRange(HitboxData hitboxData)
    {
        var collider = meleeHitbox.GetComponent<BoxCollider>();
        var indicator = hitboxRangeIndicator.transform;

        collider.center = hitboxData.hitboxPosition;
        collider.size = hitboxData.hitboxScale;

        indicator.localPosition = new Vector3(hitboxData.hitboxPosition.x, 0f, hitboxData.hitboxPosition.z);
        indicator.localScale = hitboxData.hitboxScale;
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
