using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float DestrucTime;
    void Start()
    {
        Destroy(gameObject, DestrucTime);
    }
}