using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatVisualizer : MonoBehaviour
{
    private MusicPlayer musicPlayer;

    public RectTransform beatContainer;
    public GameObject beatUIPrefab;

    public float beatTravelDistance; //this can be calculated by (width of beatContainerUI) / (number of beat UIs in the container)
    public float slideSpeed;

    private void Start()
    {
        musicPlayer = GetComponent<MusicPlayer>();
        slideSpeed = beatTravelDistance / (60f / musicPlayer.musicBPM); // travel distance divided by secPerBeat;
        
    }

    private void Update()
    {
        SlideBeatContainer();
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
