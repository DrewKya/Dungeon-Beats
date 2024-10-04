using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public UnityEvent onPress;
    public UnityEvent onRelease;

    private void TogglePlate(bool isPressed)
    {
        if (isPressed)
        {
            onPress.Invoke();
        }
        else
        {
            onRelease.Invoke();
        }
    }

    private void PlayPressedAnimation()
    {
        animator.SetTrigger("Pressed");
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.GetComponent<Rigidbody>() != null)
        {
            PlayPressedAnimation();
            TogglePlate(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.GetComponent<Rigidbody>() != null)
        {
            TogglePlate(false);
        }
    }
}
