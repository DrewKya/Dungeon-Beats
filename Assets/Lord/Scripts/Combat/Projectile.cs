using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    MeleeHitboxTrigger hitbox;

    void Start()
    {
        hitbox = GetComponent<MeleeHitboxTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
