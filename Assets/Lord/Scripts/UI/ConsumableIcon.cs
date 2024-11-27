using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableIcon : MonoBehaviour
{
    public Image icon;
    public Image backgroundFill;

    public void SetItem(Consumable consumable)
    {
        if (consumable != null)
        {
            icon.sprite = consumable.icon;
            icon.enabled = true;
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
        }
    }
}
