using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//exits the game when this object is clicked
public class OnClickExit : MonoBehaviour
{
    private void Start()
    {
        if(GetComponent<Button>() != null)
        {
            GetComponent<Button>().onClick.AddListener(() => QuitGame());
        }
    }

    private void OnMouseDown()
    {
        QuitGame();
    }

    private void QuitGame()
    {
        GameManager.instance.QuitApplication();
    }
}
