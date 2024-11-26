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

    public void SetDamage(int damage, bool isCrit)
    {
        if(textMesh == null) textMesh = GetComponent<TMP_Text>();
        

        if (!isCrit)
        {
            textMesh.text = damage.ToString();
            textMesh.fontStyle = FontStyles.Normal;
            textMesh.fontSize = 36;
        }
        else
        {
            textMesh.text = damage.ToString() + "!";
            textMesh.fontStyle = FontStyles.Bold;
            textMesh.fontSize = 50;
        }
    }

    public IEnumerator DisablePopupAfterDelay()
    {
        yield return new WaitForSeconds(popupDuration);
        gameObject.SetActive(false);
    }
}
