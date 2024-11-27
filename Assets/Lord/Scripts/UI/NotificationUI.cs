using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class NotificationUI : MonoBehaviour
{
    public static NotificationUI instance { get; private set; }

    [SerializeField] GameObject panel;
    [SerializeField] NotificationPanel notificationPanel;
    [SerializeField] Sprite exclamationMark;

    private Vector2 initialPosition;
    private Sequence currentSequence;


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
        notificationPanel.SetItemData(item);
        AnimatePanel();
    }

    public void TextNotification(string text)
    {
        notificationPanel.SetTextData(text, exclamationMark);
        AnimatePanel();
    }

    private void AnimatePanel()
    {
        // Stop any ongoing tween and reset position
        if (currentSequence != null && currentSequence.IsActive())
        {
            currentSequence.Kill(false);
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

        sequence.OnComplete(ResetPanelState);

        currentSequence = sequence;
    }

    private void ResetPanelState()
    {
        RectTransform rectTransform = panel.GetComponent<RectTransform>();

        rectTransform.anchoredPosition = initialPosition;
        panel.SetActive(false);
    }

}

[Serializable]
public class NotificationPanel
{
    public Image icon;
    public TMP_Text notificationText;

    public void SetItemData(Item item)
    {
        icon.sprite = item.icon;
        notificationText.text = item.itemName;
    }

    public void SetTextData(string _text, Sprite _sprite)
    {
        icon.sprite = _sprite;
        notificationText.text = _text;
    }
}
