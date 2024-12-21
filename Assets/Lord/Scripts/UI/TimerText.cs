using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// This script is for showing map timer on the UI

public class TimerText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text collapseText;

    [SerializeField] private GameObject collapsePanel;
    private float remainingTime;

    private void Start()
    {
        collapsePanel.SetActive(false);

        remainingTime = MapManager.instance.GetTimeLimit();

        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        while (remainingTime > 0)
        {
            if(remainingTime <= 60)
            {
                if(remainingTime > 55)
                {
                    text.gameObject.SetActive(false);
                    collapsePanel.SetActive(true);
                }

                collapseText.text = remainingTime.ToString("F0");
            }
            else
            {
                int minutes = Mathf.FloorToInt(remainingTime / 60);
                int seconds = Mathf.FloorToInt(remainingTime % 60);

                text.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
            }

            yield return new WaitForSeconds(1f);

            remainingTime -= 1f;
        }

        text.text = "0";
    }
}

