using UnityEngine;

public class VFXCollision : MonoBehaviour
{
    public GameObject particleEffectPrefab; // Assign your particle effect prefab in the inspector

    private void OnTriggerEnter(Collider collision)
    {
        // Replace OnCollisionEnter with OnTriggerEnter if using IsTrigger
        SpawnParticleEffect();
        Destroy(gameObject); // Destroy the current prefab
    }

    private void SpawnParticleEffect()
    {
        if (particleEffectPrefab != null)
        {
            // Instantiate particle effect at the prefab's position and rotation
            Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Particle Effect Prefab is not assigned!");
        }
    }
}
