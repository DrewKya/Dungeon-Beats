using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This enemy AI moves in a random direction without following the player
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

    protected override void Move()
    {
        List<MoveDirection> availableDirections = new List<MoveDirection>((MoveDirection[])Enum.GetValues(typeof(MoveDirection)));
        Vector3 positionIncrement = Vector3.zero;

        while (availableDirections.Count > 0)
        {
            MoveDirection direction = availableDirections[UnityEngine.Random.Range(0, availableDirections.Count)]; //pick one random

            positionIncrement = SetPositionIncrement(direction, positionIncrement);

            Vector3 targetTilePosition = groundCheck.position + positionIncrement;

            if (CheckIfWalkable(targetTilePosition, positionIncrement))
            {
                RotateEntity(positionIncrement);
                animator.SetTrigger("Move");
                transform.position += positionIncrement;
                return;
            }

            availableDirections.Remove(direction);
        }
    }
}
