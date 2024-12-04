using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance { get; private set; }

    public AudioMixerGroup mixerGroup;
    public int audioSourcePoolSize = 10;

    private AudioSource[] audioSources;
    private int currentIndex = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"More than one instance of {instance.GetType()} found!");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeAudioPool();
    }

    private void InitializeAudioPool()
    {
        audioSources = new AudioSource[audioSourcePoolSize];
        for (int i = 0; i < audioSourcePoolSize; i++)
        {
            GameObject audioObject = new GameObject($"SFX_AudioSource_{i}");
            audioObject.transform.SetParent(transform);

            AudioSource newAudioSource = audioObject.AddComponent<AudioSource>();
            newAudioSource.outputAudioMixerGroup = mixerGroup;
            newAudioSource.playOnAwake = false;
            audioSources[i] = newAudioSource;
        }
    }

    public void PlaySFX(AudioClip clip, Vector3 position)
    {
        AudioSource audioSource = audioSources[currentIndex];
        currentIndex = (currentIndex + 1) % audioSourcePoolSize;

        audioSource.transform.position = position;
        audioSource.clip = clip;
        audioSource.Play();
    }
}
