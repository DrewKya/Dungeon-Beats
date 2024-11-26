using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class NotificationUI : MonoBehaviour
{
    public static NotificationUI instance { get; private set; }

    [SerializeField] GameObject panel;
    [SerializeField] ItemObtainPanel itemObtainData;

    private Vector2 initialPosition;
    private Tweener currentTween;

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

    private void Start()
    {
        initialPosition = panel.GetComponent<RectTransform>().anchoredPosition;
        panel.SetActive(false);
    }

    public void ItemObtainedNotification(Item item)
    {
        itemObtainData.SetData(item);

        // Stop any ongoing tween and reset position
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
            panel.GetComponent<RectTransform>().anchoredPosition = initialPosition;
        }

        panel.SetActive(true);
        SlideOutPanel();
    }

    private void SlideOutPanel()
    {
        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        Vector2 targetPosition = initialPosition - new Vector2(rectTransform.rect.width * 2f, 0f);

        rectTransform.anchoredPosition = targetPosition; //start outside the screen

        Sequence sequence = DOTween.Sequence();

        sequence.Append(rectTransform.DOAnchorPos(initialPosition, 0.2f).SetEase(Ease.InOutSine)); // slide in to screen
        sequence.AppendInterval(3f); // Add a delay of 3 seconds
        sequence.Append(rectTransform.DOAnchorPos(targetPosition, 1f).SetEase(Ease.InOutSine)); // slide out of screen

        sequence.OnComplete(() =>
        {
            rectTransform.anchoredPosition = initialPosition; // Reset position on animation complete
            panel.SetActive(false);
        });
    }

}

[Serializable]
public class ItemObtainPanel
{
    public Image itemIcon;
    public TMP_Text itemName;

    public void SetData(Item item)
    {
        itemIcon.sprite = item.icon;
        itemName.text = item.itemName;
    }
}
