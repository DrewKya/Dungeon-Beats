using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.VFX;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] GameObject playerModel;

    [SerializeField] Animator characterAnimator;
    [SerializeField] PlayableDirector ultDirector;

    public Animator weaponAnimation;
    public VisualEffect weaponVFX;


    private Weapon.AnimationType currentAnimationType = 0;

    public void RotatePlayer(Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction);
        gameObject.transform.rotation = rotation;
    }

    public IEnumerator HopAnimation(Vector3 firstPosition, Vector3 targetPosition)
    {
        float hopHeight = 1f;
        float time = 0.1f;
        float timeElapsed = 0f;

        Vector3 hopPosition = firstPosition + ((targetPosition - firstPosition) * 0.5f) + new Vector3(0, hopHeight, 0);

        playerModel.transform.position = firstPosition;

        // Go up
        while (timeElapsed < time)
        {
            playerModel.transform.position = Vector3.Lerp(firstPosition, hopPosition, timeElapsed / time);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        playerModel.transform.position = hopPosition;

        timeElapsed = 0f;

        // Go down
        while (timeElapsed < time)
        {
            playerModel.transform.position = Vector3.Lerp(hopPosition, targetPosition, timeElapsed / time);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        playerModel.transform.position = targetPosition;
    }

    public void PlayAttackAnimation()
    {
        characterAnimator.SetTrigger("Attack");
        
        if(weaponVFX != null)
        {
            weaponVFX.Play();
        }
        else if(weaponAnimation != null)
        {
            weaponAnimation.SetTrigger("Attack");
        }
    }

    public void PlayUltimateAnimation()
    {
        ultDirector.Play();
    }

    public void SetAnimationType(Weapon.AnimationType type)
    {
        if(currentAnimationType == type)
        {
            return;
        }

        currentAnimationType = type;
        characterAnimator.SetInteger("WeaponType", (int)currentAnimationType);
        characterAnimator.SetTrigger("ChangeWeapon");
    }
}
