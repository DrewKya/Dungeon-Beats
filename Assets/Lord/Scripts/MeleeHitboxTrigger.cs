using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitboxTrigger : MonoBehaviour
{
    public int damage;
    public bool isCrit = false;

    private void OnTriggerEnter(Collider collider)
    {
        IDamageable damageable = collider.GetComponent<IDamageable>();
        if(damageable != null)
        {
            damageable.TakeDamage(damage, isCrit);
        }
    }
}
