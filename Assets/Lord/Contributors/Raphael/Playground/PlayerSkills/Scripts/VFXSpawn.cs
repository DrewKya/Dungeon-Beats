using UnityEngine;
using UnityEngine.VFX;

public class VFXSpawn : MonoBehaviour
{
    public VisualEffect vfxPrefab;  
    public LayerMask groundLayer;   
    public Vector3 positionOffset;  

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {

                Vector3 spawnPosition = hit.point + positionOffset;


                VisualEffect spawnedVFX = Instantiate(vfxPrefab, spawnPosition, Quaternion.identity);


                spawnedVFX.Play();


                Destroy(spawnedVFX.gameObject, 1f);
            }
        }
    }
}
