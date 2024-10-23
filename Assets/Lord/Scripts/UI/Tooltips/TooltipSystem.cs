using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    #region Singleton
    public static TooltipSystem instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"More than one instance of {instance.GetType()} found!");
            Destroy(gameObject);
            return;
        }
        instance = this;
        
    }
    #endregion

    public ItemTooltip itemTooltip;

    public void Show(Item item)
    {
        itemTooltip.SetContent(item);
        itemTooltip.gameObject.SetActive(true);
    }

    public void Hide()
    {
        if(itemTooltip != null) itemTooltip.gameObject.SetActive(false);
    }
}
