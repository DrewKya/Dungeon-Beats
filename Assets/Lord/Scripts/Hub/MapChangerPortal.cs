using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MapChangerPortal : MonoBehaviour
{
    [SerializeField] string sceneName;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if(SceneLoader.instance != null)
            {
                SceneLoader.instance.LoadScene(sceneName);
            }
            else
            {
                Debug.Log("Could not find a scene loader instance");
                SceneManager.LoadScene(sceneName);
            }
            
        }
    }
}
