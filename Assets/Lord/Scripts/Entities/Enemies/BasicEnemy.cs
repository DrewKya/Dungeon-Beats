using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    [SerializeField] private int actionCooldown = 2;

    private int cooldown = 0;

    public override void TakeAction()
    {
        if(cooldown <= 0)
        {
            cooldown = actionCooldown - 1;
            Move();
        }
        else
        {
            cooldown--;
        }
    }
}
