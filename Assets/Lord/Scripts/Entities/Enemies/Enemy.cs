using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditorInternal;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IActionable, IDamageable
{
    [SerializeField] protected GameObject model;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform groundCheck;

    public int maxHealthPoint = 10;
    public int healthPoint;
    public int attack = 1;

    public int coinDropped = 1;

    private void Start()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
        healthPoint = maxHealthPoint;
    }

    private bool isRendererVisible()
    {
        if(model.GetComponent<Renderer>().isVisible)
        {
            return true;
        }
        return false;
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

    public enum direction
    {
        up, down, left, right
    };

    protected virtual void Move()
    {
        List<direction> availableDirections = new List<direction>((direction[])Enum.GetValues(typeof(direction)));
        Vector3 positionIncrement = Vector3.zero;

        while ( availableDirections.Count > 0)
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

    protected void RotateEntity(Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction);
        gameObject.transform.rotation = rotation;
    }

    protected void RotateEntity(direction direction)
    {
        Vector3 dir = Vector3.zero;
        
        switch (direction)
        {
            case direction.up:
                dir = Vector3.forward;
                break;
            case direction.down:
                dir = Vector3.back; 
                break;
            case direction.left:
                dir = Vector3.left; 
                break;
            case direction.right:
                dir = Vector3.right; 
                break;
        }

        Quaternion rotation = Quaternion.LookRotation(dir);
        gameObject.transform.rotation = rotation;
    }

    protected IEnumerator HopAnimation(Vector3 firstPosition, Vector3 targetPosition)
    {
        float hopHeight = 1f;
        float time = 0.1f;
        float timeElapsed = 0f;

        Vector3 hopPosition = firstPosition + ((targetPosition - firstPosition) * 0.5f) + new Vector3(0, hopHeight, 0);

        model.transform.position = firstPosition;

        // Go up
        while (timeElapsed < time)
        {
            model.transform.position = Vector3.Lerp(firstPosition, hopPosition, timeElapsed / time);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        model.transform.position = hopPosition;

        timeElapsed = 0f;

        // Go down
        while (timeElapsed < time)
        {
            model.transform.position = Vector3.Lerp(hopPosition, targetPosition, timeElapsed / time);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        model.transform.position = targetPosition;
    }

    protected Vector3 SetPositionIncrement(direction direction, Vector3 increment)
    {
        switch (direction)
        {
            case direction.up:
                increment = new Vector3(0, 0, 1);
                break;
            case direction.down:
                increment = new Vector3(0, 0, -1);
                break;
            case direction.left:
                increment = new Vector3(1, 0, 0);
                break;
            case direction.right:
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

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        GetComponent<Collider>().enabled = false;
        EntityManager.instance.RemoveEntity(this);
        PlayerManager.instance.AddCoin(coinDropped);
        animator.SetTrigger("Die");
        Destroy(gameObject, 2f);
    }
}
