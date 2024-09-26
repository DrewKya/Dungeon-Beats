using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Item : ScriptableObject
{
    public string itemName = "Item Name";
    public string itemDescription = "New description";
    public Sprite icon = null;

    public virtual void Use() { }
    public virtual void Drop()
    {
        InventoryManager.instance.RemoveItem(this);
    }
}
