using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float shakeAmount = 0.02f;
    private Vector3 initialPos;
    void Awake()
    {
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = initialPos + Random.insideUnitSphere * shakeAmount;
    }
}
