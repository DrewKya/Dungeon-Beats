using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.VFX;

public class PlayerEntity : MonoBehaviour, IDamageable
{
    private PlayerManager playerManager;
    private PlayerAnimation playerAnimation;
    private IngameParametersUI parametersUI;

    public PlayerStats stats;
    public int currentHP;

    [SerializeField] private GameEvent onPlayerTakeDamage;

    public UltimateData BaseUltimate;
    public UltimateData AwakeningUltimate;

    private Weapon selectedWeapon;
    public GameObject hitboxRangeIndicator;
    public MeleeHitboxTrigger meleeHitbox;

    public Transform weaponAttachPoint;
    public Transform offhandAttachPoint;
    public Transform VFX_AttachPoint;

    public float nextAttackTime; //determines the next Time.time the player can attack
    public bool isCharging; //determines if player is charging an attack
    private bool isAwakened = false;
    private bool isImmune = false;

    [SerializeField] AudioClip damageSFX; //play on player taking damage

    private void Start()
    {
        if(VFX_AttachPoint== null)
        {
            Debug.LogWarning("VFX attach point is null, make sure to put a reference to it");
        }

        playerAnimation = GetComponent<PlayerAnimation>();

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
        parametersUI.UpdateHealthPointsUI(currentHP, stats.healthPoint);
        
        SetWeaponModel();
        
    }

    private void SetWeaponModel()
    {
        //remove existing weapon model
        if(weaponAttachPoint.childCount > 0)
        {
            foreach(Transform child in weaponAttachPoint.transform)
            {
                Destroy(child.gameObject);
            }
            foreach(Transform child in offhandAttachPoint.transform) 
            { 
                Destroy(child.gameObject); 
            }
            foreach(Transform child in VFX_AttachPoint.transform)
            {
                Destroy(child.gameObject);
            }
        }

        //remove existing weapon vfx and animation references
        playerAnimation.weaponVFX = null;
        playerAnimation.weaponAnimation = null;

        if (playerManager.currentWeapon != null)
        {
            playerAnimation.SetAnimationType(playerManager.currentWeapon.animationType);
        }
        else
        {
            playerAnimation.SetAnimationType(0);
        }

        //initialize weapon based on its type
        if (playerManager.currentWeapon is MeleeWeapon)
        {
            MeleeWeapon meleeWeapon = (MeleeWeapon)playerManager.currentWeapon;
            meleeWeapon.Initialize(weaponAttachPoint, offhandAttachPoint, VFX_AttachPoint);
        }
        else if (playerManager.currentWeapon is RangedWeapon)
        {
            RangedWeapon rangedWeapon = (RangedWeapon)playerManager.currentWeapon;
            rangedWeapon.Initialize(weaponAttachPoint, offhandAttachPoint);
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

        selectedWeapon = playerManager.currentWeapon;
        if(selectedWeapon == null)
        {
            NotificationUI.instance.TextNotification("No weapon equipped!");
            return;
        }

        if (!CheckAttackCooldown())
        {
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
            StartCoroutine(ToggleHitbox());
            SetMeleeAttackVFX();

        }
        else if (selectedWeapon is RangedWeapon)
        {
            RangedWeapon weapon = (RangedWeapon)selectedWeapon;
            ShootProjectile(weapon.projectilePrefab, weapon.projectileSpeed);
        }

        playerAnimation.PlayAttackAnimation();

        StartCoroutine(parametersUI.weaponIcon.StartCooldown(selectedWeapon.attackCooldownInSeconds));
        nextAttackTime = Time.time + selectedWeapon.attackCooldownInSeconds;
        hitboxRangeIndicator.SetActive(false);
        isCharging = false;
    }

    private void SetMeleeAttackVFX()
    {
        if (playerAnimation.weaponVFX == null)
        {
            playerAnimation.weaponVFX = VFX_AttachPoint.GetComponentInChildren<VisualEffect>();

            // If no VisualEffect is found, try to get an Animator component
            if (playerAnimation.weaponVFX == null)
            {
                playerAnimation.weaponAnimation = VFX_AttachPoint.GetComponentInChildren<Animator>();
            }
        }

        VFX_AttachPoint.transform.position = this.transform.position;
        VFX_AttachPoint.transform.rotation = this.transform.rotation;
    }

    public void UseItem()
    {
        if(playerManager.currentConsumable == null)
        {
            NotificationUI.instance.TextNotification("No consumable item equipped!");
            return;
        }

        playerManager.currentConsumable.UseConsumable(this);
        playerManager.ConsumeItem();
    }
    public void PreviewUltimate()
    {
        if (!isAwakened)
        {
            CheckHitboxRange(BaseUltimate.hitboxPreviewData);

        }
        else
        {
            CheckHitboxRange(AwakeningUltimate.hitboxPreviewData);
        }
        isCharging = true;
        hitboxRangeIndicator.SetActive(true);
    }

    public void UltimateAttack()
    {
        isCharging = false;
        hitboxRangeIndicator.SetActive(false);

        if (!isAwakened)
        {
            CalculateDamageDealt(BaseUltimate.hitboxTrigger);
            BaseUltimate.PlayUltimate();
        }
        else
        {
            CalculateDamageDealt(AwakeningUltimate.hitboxTrigger);
            AwakeningUltimate.PlayUltimate();
        }
    }

    public IEnumerator AwakenModeCoroutine()
    {
        isImmune = true;
        isAwakened = true;
        playerAnimation.AwakenAnimation(true);

        yield return new WaitForSeconds(3f);

        isImmune = false;

        yield return new WaitForSeconds(7f);

        isAwakened = false;
        playerAnimation.AwakenAnimation(false);
    }
    public void EnterAwakenMode()
    {
        StartCoroutine(AwakenModeCoroutine());
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
        CalculateDamageDealt(meleeHitbox);
        meleeHitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        meleeHitbox.gameObject.SetActive(false);
    }

    public void ShootProjectile(GameObject projectilePrefab, float projectileSpeed)
    {
        Vector3 spawnPosition = this.transform.position + this.transform.forward * 1f + Vector3.up * 0.5f;

        var projectile = Instantiate(projectilePrefab, spawnPosition, transform.rotation);

        projectile.GetComponent<Rigidbody>().velocity = this.transform.forward * projectileSpeed;
        Destroy(projectile, 5f);

        RangedHitboxTrigger projectileHitbox = projectile.GetComponent<RangedHitboxTrigger>(); 
        CalculateDamageDealt(projectileHitbox);
    }

    private void CalculateDamageDealt(HitboxTrigger hitbox)
    {
        float critRoll = UnityEngine.Random.Range(0f, 100f);
        if(critRoll < stats.critRate)
        {
            hitbox.isCrit = true;
            hitbox.damage = Mathf.FloorToInt(stats.attack * 1.5f); //crit damage
        }
        else
        {
            hitbox.isCrit = false;
            hitbox.damage = stats.attack;
        }
    }

    public void TakeDamage(int damage, bool isCrit = false)
    {
        int totalDamage = CalculateDamageTaken(damage);

        PopupPool.instance.ShowDamage(transform.position, totalDamage, isCrit);
        currentHP -= totalDamage;

        onPlayerTakeDamage.TriggerEvent();
        SFXManager.instance.PlaySFX(damageSFX, transform.position);

        parametersUI.UpdateHealthPointsUI(currentHP, stats.healthPoint);

        if(currentHP < 0)
        {
            PlayerDie();
        }
    }

    public void Heal(int healAmount)
    {
        int healedHP = Math.Min(currentHP + healAmount, stats.healthPoint);
        currentHP = healedHP;

        parametersUI.UpdateHealthPointsUI(currentHP, stats.healthPoint);
    }

    private int CalculateDamageTaken(int damage)
    {
        if (isImmune)
        {
            return 0;
        }

        return Math.Max(1, damage - stats.defense);
    }

    public void PlayerDie()
    {
        Debug.Log("Player died!");
    }
}
