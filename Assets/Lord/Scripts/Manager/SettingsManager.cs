using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance {  get; private set; }

    [SerializeField] AudioMixer audioMixer;

    public Resolution[] resolutions;
    public int currentResolutionIndex = 0;
    public bool isFullscreen = true;

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
    }

    private void Start()
    {
        if (audioMixer == null)
        {
            Debug.LogWarning("Audio mixer not set in inspector");
            audioMixer = GetComponent<AudioMixer>();
        }

        InitializeScript();
    }
    private void InitializeScript()
    {
        Application.targetFrameRate = 60;

        resolutions = new Resolution[]
        {
            new Resolution { width = 1920, height = 1080 },
            new Resolution { width = 1600, height = 900 },
            new Resolution { width = 1280, height = 720 }
        };

        LoadFromPlayerPrefs();
    }

    private void LoadFromPlayerPrefs()
    {
        SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 0.8f));
        SetBGMVolume(PlayerPrefs.GetFloat("BGMVolume", 0.8f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 0.8f));

        currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1; //convert int to boolean

        SetResolution(currentResolutionIndex);
    }

    public void SetResolution(int index)
    {
        if (index < 0 || index >= resolutions.Length)
        {
            Debug.LogError($"Invalid resolution index: {index}");
            return;
        }

        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, GetFullScreenMode());

        currentResolutionIndex = index;
    }

    private FullScreenMode GetFullScreenMode()
    {
        return isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    private void SetVolume(string parameterName, float value)
    {
        if (value <= 0.05f)
        {
            audioMixer.SetFloat(parameterName, -80f);
            return;
        }

        audioMixer.SetFloat(parameterName, Mathf.Log10(value) * 20);
    }

    public void SetMasterVolume(float value)
    {
        SetVolume("MasterVolume", value);
    }
    public void SetBGMVolume(float value)
    {
        SetVolume("bgmVolume", value);
    }
    public void SetSFXVolume(float value)
    {
        SetVolume("sfxVolume", value);
    }
}
