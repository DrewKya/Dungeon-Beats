using UnityEngine;
using UnityEngine.SceneManagement;

public class MapChangerPortal : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] GameObject activeModel;
    [SerializeField] GameObject inactiveModel;

    [SerializeField] bool startActive = true;

    private void Start()
    {
        if (startActive)
        {
            ActivatePortal();
        }
        else
        {
            DisablePortal();
        }
    }

    public void ActivatePortal()
    {
        inactiveModel?.SetActive(false);
        activeModel?.SetActive(true);

        GetComponent<Collider>().enabled = true;
    }

    public void DisablePortal()
    {
        inactiveModel?.SetActive(true);
        activeModel?.SetActive(false);

        GetComponent<Collider>().enabled = false;
    }

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
