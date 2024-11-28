using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutinTransition : MonoBehaviour
{
    public Material material;
    public float transitionDuration;

    string transitionValue = "_AlphaTest";

    public void SetTransitionValue(float value)
    {
        material.SetFloat(transitionValue, value);
    }

    private void OnEnable()
    {
        StartCoroutine(LerpTransition(transitionDuration));
    }

    private IEnumerator LerpTransition(float duration)
    {
        material.SetFloat (transitionValue, 3);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float currentValue  = Mathf.Lerp(3f, -1.2f, elapsedTime/duration);

            material.SetFloat(transitionValue, currentValue);

            yield return null;
        }
    }
}
