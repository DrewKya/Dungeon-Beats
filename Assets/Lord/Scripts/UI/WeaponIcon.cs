using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponIcon : MonoBehaviour
{
    public Image icon;
    public Image backgroundFill;
    public TMP_Text cooldownRemaining;

    public void SetWeapon(Weapon weapon)
    {
        if(weapon != null)
        {
            icon.sprite = weapon.icon;
            icon.enabled = true;
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
        }
        
    }

    public IEnumerator StartCooldown(float cooldown)
    {
        float time = cooldown;

        while (time > 0f)
        {
            time -= Time.deltaTime;
            cooldownRemaining.text = Mathf.CeilToInt(time).ToString();
            backgroundFill.fillAmount = 1- (time / cooldown);

            yield return null;
        }

        backgroundFill.fillAmount = 1;
        cooldownRemaining.text = "<sprite=56>";
    }
}
