using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageVignette : MonoBehaviour
{
    [SerializeField] Image vignette;

    private void Start()
    {
        vignette.gameObject.SetActive(false);
    }

    public void OnTakeDamage()
    {
        StopAllCoroutines();
        StartCoroutine(ShowVignette());
    }

    private IEnumerator ShowVignette() //make the vignette show, then fades out
    {
        float duration = 0.5f;  // The duration of the fade
        float elapsedTime = 0f;

        Color initialColor = new Color(vignette.color.r, vignette.color.g, vignette.color.b, 1f);
        Color transparentColor = new Color(vignette.color.r, vignette.color.g, vignette.color.b, 0f);

        vignette.color = initialColor;
        vignette.gameObject.SetActive(true);

        while (elapsedTime < duration)
        {
            vignette.color = Color.Lerp(initialColor, transparentColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        vignette.color = transparentColor;
        vignette.gameObject.SetActive(false);
    }
}
