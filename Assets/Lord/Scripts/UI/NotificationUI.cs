using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationUI : MonoBehaviour
{
    public static NotificationUI instance { get; private set; }

    [SerializeField] GameObject panel;
    [SerializeField] ItemObtainPanel itemObtainData;

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
        panel.SetActive(false);
    }

    public void ItemObtainedNotification(Item item)
    {
        itemObtainData.SetData(item);
        StartCoroutine(ShowItemObtained());

    }

    private IEnumerator ShowItemObtained() //make the panel show, then fades out
    {
        float duration = 4f;  // The duration of the fade
        float elapsedTime = 0f;

        Image obj = panel.GetComponent<Image>();

        Color initialColor = new Color(obj.color.r, obj.color.g, obj.color.b, 1f);
        Color transparentColor = new Color(obj.color.r, obj.color.g, obj.color.b, 0f);

        obj.color = initialColor;
        obj.gameObject.SetActive(true);

        while (elapsedTime < duration)
        {
            obj.color = Color.Lerp(initialColor, transparentColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.color = transparentColor;
        obj.gameObject.SetActive(false);
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