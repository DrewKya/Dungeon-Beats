using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateHitboxTrigger : HitboxTrigger
{
    private void OnTriggerEnter(Collider collider)
    {

        IDamageable damageable = collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(Mathf.RoundToInt((float)damage * damageScaling), isCrit);
            SFXManager.instance.PlaySFX(hitSFX, transform.position);
        }
    }
}
