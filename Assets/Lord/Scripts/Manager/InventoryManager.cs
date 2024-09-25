using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region Singleton
    public static InventoryManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning($"More than one instance of {instance.GetType()} found!");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public List<Item> items = new List<Item>();
    public int maxSlot = 20;

    public bool AddItem(Item item)
    {
        if (items.Count >= maxSlot)
        {
            Debug.Log("No space in inventory");
            return false;
        }
        items.Add(item);
        Debug.Log($"[{item.itemName}] added to inventory");
        return true;
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }
}
