using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnClickGameObject : MonoBehaviour
{
    [SerializeField] UnityEvent onClickEvent;

    private void OnMouseDown()
    {
        onClickEvent.Invoke();
    }
}
