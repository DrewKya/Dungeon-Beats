using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// This script is for showing map timer on the UI

[RequireComponent(typeof(TMP_Text))]
public class TimerText : MonoBehaviour
{
    private TMP_Text text;
    private float remainingTime;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        remainingTime = MapManager.instance.GetTimeLimit();

        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        while (remainingTime > 0)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

            text.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);

            yield return new WaitForSeconds(1f);

            remainingTime -= 1f;
        }

        text.text = "00:00";
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

