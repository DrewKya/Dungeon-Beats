using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;

    public AudioSource audioSource;
    
    public float musicBPM;
    public Interval interval;
    public float songPositionInBeats;

    public float intervalLength;

    public TMP_Text timingText;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning($"More than one instance of {Instance.GetType()} found!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        intervalLength = interval.GetIntervalLength(musicBPM);
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    private void Update()
    {
        songPositionInBeats = audioSource.timeSamples / (audioSource.clip.frequency * intervalLength);
        interval.CheckNewInterval(songPositionInBeats);
    }
}

[System.Serializable]
public class Interval
{
    [SerializeField] public UnityEvent trigger;

    public int lastInterval; //track the last interval

    public float GetIntervalLength(float bpm)
    {
        return 60f / bpm;
    }

    public void CheckNewInterval(float interval)
    {
        var roundedInterval = Mathf.FloorToInt(interval); //round down

        if (roundedInterval != lastInterval)
        {
            lastInterval = roundedInterval;
            trigger.Invoke();
        }
    }
}