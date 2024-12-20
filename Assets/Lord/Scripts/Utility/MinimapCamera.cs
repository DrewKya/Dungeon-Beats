using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField] private GameObject targetPlayer;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        var newPosition = new Vector3(targetPlayer.transform.position.x,
                                   transform.position.y,
                                   targetPlayer.transform.position.z);

        transform.position = newPosition;
    }
}
