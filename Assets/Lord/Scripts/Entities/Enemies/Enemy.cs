using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditorInternal;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IActionable, IDamageable
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform groundCheck;

    public int maxHealthPoint = 10;
    public int healthPoint;

    public int coinDropped = 1;

    private void Start()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
        healthPoint = maxHealthPoint;
    }

    public virtual void TakeAction()
    {
        Move();
    }
    public virtual void TakeDamage(int damage, bool isCrit = false)
    {
        PopupPool.instance.ShowDamage(transform.position, damage, isCrit);
        healthPoint -= Math.Max(0, damage);
        if ( healthPoint <= 0)
        {
            Die();
        }
    }

    public enum MoveDirection
    {
        up, down, left, right
    };

    protected virtual void Move()
    {
        List<MoveDirection> availableDirections = new List<MoveDirection>((MoveDirection[])Enum.GetValues(typeof(MoveDirection)));
        Vector3 positionIncrement = Vector3.zero;

        while ( availableDirections.Count > 0)
        {
            MoveDirection direction = availableDirections[UnityEngine.Random.Range(0, availableDirections.Count)]; //pick one random
            
            positionIncrement = SetPositionIncrement(direction, positionIncrement);

            Vector3 targetTilePosition = groundCheck.position + positionIncrement;

            if (CheckIfWalkable(targetTilePosition, positionIncrement))
            {
                RotateEntity(positionIncrement);
                transform.position += positionIncrement;
                return;
            }
            
            availableDirections.Remove(direction);
        }  
    }

    protected void RotateEntity(Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction);
        gameObject.transform.rotation = rotation;
    }

    protected Vector3 SetPositionIncrement(MoveDirection direction, Vector3 increment)
    {
        switch (direction)
        {
            case MoveDirection.up:
                increment = new Vector3(0, 0, 1);
                break;
            case MoveDirection.down:
                increment = new Vector3(0, 0, -1);
                break;
            case MoveDirection.left:
                increment = new Vector3(1, 0, 0);
                break;
            case MoveDirection.right:
                increment = new Vector3(-1, 0, 0);
                break;
            default:
                break;
        }
        return increment;
    }

    protected bool CheckIfWalkable(Vector3 target, Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(groundCheck.position, direction, out hit, 1f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) //check if there is a collider in that direction
        {
            return false;
        }

        return (Physics.Raycast(target, Vector3.down, out hit, 1f, LayerMask.GetMask("Ground"))) ? true : false; //check if ground exist in that direction
    }

    public virtual void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        EntityManager.instance.RemoveEntity(this);
        PlayerManager.instance.AddCoin(coinDropped);
        Destroy(gameObject);
    }
}
