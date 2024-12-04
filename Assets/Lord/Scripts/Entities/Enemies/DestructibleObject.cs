using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject destroyedModel;

    public int hitsRemaining = 3;

    public virtual void TakeDamage(int damage, bool isCrit)
    {
        PopupPool.instance.ShowDamage(transform.position, damage, isCrit);
        hitsRemaining -= 1;
        if (hitsRemaining <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        model.SetActive(false);
        Instantiate(destroyedModel, transform.position, Quaternion.identity, this.transform);
        Destroy(gameObject, 2f);
    }
}
