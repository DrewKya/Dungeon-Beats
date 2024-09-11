using UnityEngine;

public class BackButton : MonoBehaviour
{
    public GameObject[] saveMenusObjects;
    private Animator[] animators;

    public GameObject[] mainMenuObjects;
    private Animator[] mainMenuAnimators;

    private Animator ownAnimator;

    void Start()
    {
        animators = new Animator[saveMenusObjects.Length];
        for (int i = 0; i < saveMenusObjects.Length; i++)
        {
            if (saveMenusObjects[i] != null)
            {
                animators[i] = saveMenusObjects[i].GetComponent<Animator>();
            }
        }

        mainMenuAnimators = new Animator[mainMenuObjects.Length];
        for (int i = 0; i < mainMenuObjects.Length; i++)
        {
            if (mainMenuObjects[i] != null)
            {
                mainMenuAnimators[i] = mainMenuObjects[i].GetComponent<Animator>();
            }
        }

        ownAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    HandleClick();
                }
            }
        }
    }

    void HandleClick()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            if (animators[i] != null)
            {
                animators[i].SetBool("enterSave", false);
            }
        }

        for (int i = 0; i < mainMenuAnimators.Length; i++)
        {
            if (mainMenuAnimators[i] != null)
            {
                mainMenuAnimators[i].SetBool("disablesMenu", false);
            }
        }

        if (ownAnimator != null)
        {
            ownAnimator.SetBool("enablesBack", false);
        }
    }
}
