using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private InventorySlot slot;
    private EquipSlot equipSlot;

    private void Start()
    {
        slot = GetComponent<InventorySlot>();
        equipSlot = GetComponent<EquipSlot>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(slot != null)
        {
            if(slot.item != null) TooltipSystem.instance.Show(slot.item);
        }
        else if(equipSlot != null)
        {
            if (equipSlot.item != null) TooltipSystem.instance.Show(equipSlot.item);
        }
        else
        {
            return;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.instance.Hide();
    }

    private void OnDisable()
    {
        TooltipSystem.instance.Hide();
    }
}
