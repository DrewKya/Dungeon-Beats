using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour, IDamageable
{
    public int hitsRemaining = 3;

    public virtual void TakeDamage(int damage, bool isCrit)
    {
        PopupPool.instance.ShowDamage(transform.position, 1, isCrit);
        hitsRemaining -= 1;
        if (hitsRemaining <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
