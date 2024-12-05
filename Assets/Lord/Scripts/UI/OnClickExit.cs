using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//exits the game when this object is clicked
public class OnClickExit : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameManager.instance.QuitApplication();
    }
}
