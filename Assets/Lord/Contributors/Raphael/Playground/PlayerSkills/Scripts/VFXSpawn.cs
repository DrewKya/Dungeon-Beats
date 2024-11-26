using UnityEngine;
using UnityEngine.VFX;
using static DG.Tweening.DOTweenModuleUtils;

public class VFXSpawn : MonoBehaviour
{
    public VisualEffect vfxPrefab; 
    public LayerMask groundLayer; 
    public Vector3 positionOffset;
    public Vector3 launchDirection; 
    public float launchSpeed = 10f; 

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (UnityEngine.Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {

                Vector3 spawnPosition = hit.point + positionOffset;

                VisualEffect spawnedVFX = Instantiate(vfxPrefab, spawnPosition, Quaternion.identity);

                Rigidbody rb = spawnedVFX.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = spawnedVFX.gameObject.AddComponent<Rigidbody>();
                }


                rb.useGravity = false; 
                rb.velocity = launchDirection.normalized * launchSpeed;


                spawnedVFX.Play();


                Destroy(spawnedVFX.gameObject, 1f);
            }
        }
    }
}
