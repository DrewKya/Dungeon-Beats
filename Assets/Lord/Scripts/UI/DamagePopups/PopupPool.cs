using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PopupPool : MonoBehaviour
{
    public static PopupPool instance;
    public List<DamagePopup> pool = new List<DamagePopup>();
    public int poolSize;

    public DamagePopup popupPrefab;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pool = new List<DamagePopup>();
        DamagePopup popup;
        for(int i = 0; i < poolSize; i++)
        {
            popup = Instantiate(popupPrefab, transform);
            popup.gameObject.SetActive(false);
            pool.Add(popup);
        }
    }

    public DamagePopup GetPooledObject()
    {
        foreach(DamagePopup popup in pool)
        {
            if (!popup.gameObject.activeInHierarchy)
            {
                return popup;
            }
        }
        return null;
    }

    public void ShowDamage(Vector3 position, int damage)
    {
        DamagePopup popup = GetPooledObject();
        if(popup != null)
        {
            popup.gameObject.SetActive(true);
            popup.transform.position = position + (Random.insideUnitSphere * 0.5f);
            popup.SetDamage(damage);
            StartCoroutine(popup.DisablePopupAfterDelay());
        }
    }
}