using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IActionable, IDamageable
{
    private int numberOfDirections;

    public int healthPoints = 10;

    private void Start()
    {
        numberOfDirections = Enum.GetValues(typeof(MoveDirection)).Length;
    }

    public void TakeAction()
    {
        Move();
    }
    public void TakeDamage(int damage)
    {
        Debug.Log($"{gameObject.name} took {damage} damage!");
        healthPoints -= damage;
        if ( healthPoints <= 0)
        {
            Die();
        }
    }

    private enum MoveDirection
    {
        up, down, left, right
    };

    private void Move()
    {
        MoveDirection direction = (MoveDirection)UnityEngine.Random.Range(0, numberOfDirections);
        Vector3 positionIncrement = new Vector3(0, 0, 0);

        switch (direction)
        {
            case MoveDirection.up:
                positionIncrement = new Vector3(0, 0, 1);
                break;
            case MoveDirection.down:
                positionIncrement = new Vector3(0, 0, -1);
                break;
            case MoveDirection.left:
                positionIncrement = new Vector3(1, 0, 0);
                break;
            case MoveDirection.right:
                positionIncrement = new Vector3(-1, 0, 0);
                break;
            default:
                break;
        }

        transform.position += positionIncrement;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
