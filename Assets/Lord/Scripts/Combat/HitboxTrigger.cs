using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitboxTrigger : MonoBehaviour
{
    public int damage;
    public float damageScaling = 1f;
    public bool isCrit = false;

    [SerializeField] internal AudioClip hitSFX;
}
