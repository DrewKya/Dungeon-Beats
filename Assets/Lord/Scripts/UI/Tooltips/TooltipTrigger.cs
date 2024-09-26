using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private InventorySlot slot;

    private void Start()
    {
        slot = GetComponent<InventorySlot>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(slot.item == null) return;
        TooltipSystem.instance.Show(slot.item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.instance.Hide();
    }
}
