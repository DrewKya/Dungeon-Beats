using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedHitboxTrigger : HitboxTrigger
{
    [SerializeField] private int hitsRemaining = 1;

    private void OnTriggerEnter(Collider collider)
    {
        hitsRemaining--;

        IDamageable damageable = collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage, isCrit);
            CinemachineShake.instance.ShakeCamera(2f, 0.2f);
        }

        if(hitsRemaining <= 0)
        {
            Destroy(gameObject);
        }
    }
}
