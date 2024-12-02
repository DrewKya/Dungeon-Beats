using UnityEngine;

public class Rotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    public Vector3 rotationAxis = Vector3.up;

    [Tooltip("Speed of rotation in degrees per second.")]
    [Range(0, 500)] 
    public float rotationSpeed = 100f;

    void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
