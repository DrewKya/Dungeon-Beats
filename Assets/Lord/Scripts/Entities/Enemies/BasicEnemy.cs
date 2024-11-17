using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This enemy AI moves in a random direction without following the player
public class BasicEnemy : Enemy
{
    [SerializeField] private GameObject attackIndicator;
    [SerializeField] private MeleeHitboxTrigger hitbox;

    [SerializeField] private int actionCooldown = 2;
    private int cooldown = 0;

    private bool isReadyingAttack = false;

    public override void TakeAction()
    {
        if(cooldown <= 0)
        {
            cooldown = actionCooldown - 1;
            if(isReadyingAttack)
            {
                Attack();
                return;
            }

            direction? attackDirection = CheckPlayerInRange();
            if (attackDirection.HasValue)
            {
                ReadyAttack(attackDirection.Value);
            }
            else
            {
                Move();
            }
        }
        else
        {
            cooldown--;
        }
    }

    protected override void Move()
    {
        List<direction> availableDirections = new List<direction>((direction[])Enum.GetValues(typeof(direction)));
        Vector3 positionIncrement = Vector3.zero;

        while (availableDirections.Count > 0)
        {
            direction direction = availableDirections[UnityEngine.Random.Range(0, availableDirections.Count)]; //pick one random

            positionIncrement = SetPositionIncrement(direction, positionIncrement);

            Vector3 targetTilePosition = groundCheck.position + positionIncrement;

            if (CheckIfWalkable(targetTilePosition, positionIncrement))
            {
                RotateEntity(positionIncrement);
                StartCoroutine(HopAnimation(transform.position, transform.position + positionIncrement));

                transform.position += positionIncrement;
                return;
            }

            availableDirections.Remove(direction);
        }
    }

    protected direction? CheckPlayerInRange()
    {
        RaycastHit hit;
        int playerLayerMask = LayerMask.GetMask("Player");

        if(Physics.Raycast(transform.position, Vector3.forward, out hit, 1f, playerLayerMask))
        {
            return direction.up;
        }
        else if (Physics.Raycast(transform.position, Vector3.back, out hit, 1f, playerLayerMask))
        {
            return direction.down;
        }
        else if (Physics.Raycast(transform.position, Vector3.left, out hit, 1f, playerLayerMask))
        {
            return direction.left;
        }
        else if (Physics.Raycast(transform.position, Vector3.right, out hit, 1f, playerLayerMask))
        {
            return direction.right;
        }
        return null;
    }

    protected void ReadyAttack(direction direction)
    {
        isReadyingAttack = true;
        animator.SetTrigger("ReadyAttack");

        RotateEntity(direction);
        attackIndicator.SetActive(true);
    }

    protected void Attack()
    {
        isReadyingAttack = false;
        animator.SetTrigger("Attack");
        attackIndicator.SetActive(false);

        StartCoroutine(ToggleHitbox());
    }

    public IEnumerator ToggleHitbox()
    {
        hitbox.damage = attack;
        hitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitbox.gameObject.SetActive(false);
    }
}
