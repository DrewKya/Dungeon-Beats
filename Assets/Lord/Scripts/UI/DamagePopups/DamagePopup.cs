using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class DamagePopup : MonoBehaviour
{
    TMP_Text textMesh;

    [SerializeField] private float popupDuration = 2f;

    private void Start()
    {
        textMesh = GetComponent<TMP_Text>();
    }

    public void SetDamage(int damage)
    {
        if(textMesh == null)
        {
            textMesh = GetComponent<TMP_Text>();
        }
        textMesh.text = damage.ToString();
    }

    public IEnumerator DisablePopupAfterDelay()
    {
        yield return new WaitForSeconds(popupDuration);
        gameObject.SetActive(false);
    }
}
