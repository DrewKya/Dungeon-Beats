using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeatVisualizer : MonoBehaviour
{
    private MusicPlayer musicPlayer;

    public RectTransform beatContainer;
    public GameObject beatUIPrefab;
    public GameObject timingIndicatorUI;

    public float beatTravelDistance; //this can be calculated by (width of beatContainerUI) / (number of beat UIs in the container)
    public float slideSpeed;

    private void Start()
    {
        musicPlayer = GetComponent<MusicPlayer>();
        slideSpeed = beatTravelDistance / musicPlayer.intervalLength; // travel distance divided by secPerBeat;


    }

    private void Update()
    {
        SlideBeatContainer();
    }

    public void PulseIndicatorUI()
    {
        StartCoroutine(PulseCoroutine());
    }

    private IEnumerator PulseCoroutine()
    {
        Vector3 initialScale = new Vector3(1f, 1f, 1f);
        Vector3 pulseScale = initialScale * 1.2f;
        float duration = 0.2f;
        float elapsedTime = 0f;

        timingIndicatorUI.transform.localScale = pulseScale;

        // Scale down
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            timingIndicatorUI.transform.localScale = Vector3.Lerp(pulseScale, initialScale, t);
            yield return null;
        }
        timingIndicatorUI.transform.localScale = initialScale;
    }

    private void SlideBeatContainer()
    {
        slideSpeed = beatTravelDistance / musicPlayer.intervalLength;
        beatContainer.localPosition += Vector3.right * slideSpeed * Time.deltaTime;
        if(beatContainer.localPosition.x >= 400)
        {
            var temp = beatContainer.localPosition.x * 2;
            beatContainer.localPosition += Vector3.left * temp;
        }
    }
}
