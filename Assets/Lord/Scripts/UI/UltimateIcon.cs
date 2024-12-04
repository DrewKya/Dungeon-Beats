using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimateIcon : MonoBehaviour
{
    public Image icon;

    public void SetItem(UltimateData data)
    {
        if (data != null)
        {
            icon.sprite = data.icon;
            icon.enabled = true;
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
        }
    }
}
