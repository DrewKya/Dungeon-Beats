using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TakeDamage(1);
        }
    }
}
