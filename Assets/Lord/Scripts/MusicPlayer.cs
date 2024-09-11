using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;

    public AudioSource audioSource;

    public float musicBPM;
    public float secPerBeat;
    public float songPositionInSeconds;
    public float songPositionInBeats;

    public float dspSongTime; //seconds passed since the song started

    public Transform beatSpawnContainer;
    public GameObject beatUI;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        secPerBeat = 60f / musicBPM;
        dspSongTime = (float)AudioSettings.dspTime;

        audioSource.Play();
        //StartCoroutine(SpawnBeatIndicator());
    }

    private void Update()
    {
        songPositionInSeconds = (float)(AudioSettings.dspTime - dspSongTime);
        songPositionInBeats = songPositionInSeconds / secPerBeat;
    }

    IEnumerator SpawnBeatIndicator()
    {
        while (true)
        {
            Debug.Log("test");
            var obj = Instantiate(beatUI, beatSpawnContainer);
            obj.AddComponent<DelayedDestroy>().DelayInSeconds = secPerBeat / 2f;
            yield return new WaitForSeconds(secPerBeat);
        }
    }

    void StopLoopingCoroutine()
    {
        StopCoroutine(nameof(SpawnBeatIndicator));
    }
}
