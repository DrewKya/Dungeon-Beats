using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
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

    private void OnTriggerEnter(Collider collider)
    {
        TogglePlate(true);
    }

    private void OnTriggerExit(Collider collider)
    {
        TogglePlate(false);
    }
}
