using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    public GameObject[] saveMenusObjects;
    private Animator[] animators;

    public GameObject[] mainMenuObjects;
    private Animator[] mainMenuAnimators;

    private Animator ownAnimator;

    public GameObject BackObjects;
    private Animator specificAnimatorForEnablesBack;

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

        if (BackObjects != null)
        {
            specificAnimatorForEnablesBack = BackObjects.GetComponent<Animator>();
        }
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
                animators[i].SetBool("enterSave", true);
            }
        }

        for (int i = 0; i < mainMenuAnimators.Length; i++)
        {
            if (mainMenuAnimators[i] != null)
            {
                mainMenuAnimators[i].SetBool("disablesMenu", true);
            }
        }

        if (ownAnimator != null)
        {
            ownAnimator.SetBool("disablesMenu", true);
        }

        if (specificAnimatorForEnablesBack != null)
        {
            specificAnimatorForEnablesBack.SetBool("enablesBack", true);
        }
    }
}
