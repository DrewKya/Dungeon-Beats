using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    private SettingsManager settingsManager;

    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;

    private void Awake()
    {
        settingsManager = SettingsManager.instance;            
    }

    private void Start()
    {
        InitializeScript();
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);

        PlayerPrefs.SetInt("ResolutionIndex", settingsManager.currentResolutionIndex);
        PlayerPrefs.SetInt("Fullscreen", settingsManager.isFullscreen ? 1 : 0);
    }

    private void LoadSettingsToUI()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.8f); // Default: 80%
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.8f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        resolutionDropdown.value = settingsManager.currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.isOn = settingsManager.isFullscreen;
    }

    private void InitializeScript()
    {
        resolutionDropdown.ClearOptions();
        List<string> resolutionOptions = new List<string>();

        for (int i = 0; i < settingsManager.resolutions.Length; i++)
        {
            string option = settingsManager.resolutions[i].width + " x " + settingsManager.resolutions[i].height;
            resolutionOptions.Add(option);

            if (settingsManager.resolutions[i].width == Screen.currentResolution.width && settingsManager.resolutions[i].height == Screen.currentResolution.height)
            {
                settingsManager.currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        

        LoadSettingsToUI();
        AddListenersToUI();
    }

    private void AddListenersToUI()
    {
        fullscreenToggle.onValueChanged.AddListener(onFullscreenToggleValueChanged);
        resolutionDropdown.onValueChanged.AddListener(settingsManager.SetResolution);

        masterSlider.onValueChanged.AddListener(settingsManager.SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(settingsManager.SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(settingsManager.SetSFXVolume);
    }

    private void onFullscreenToggleValueChanged(bool isChecked)
    {
        settingsManager.isFullscreen = isChecked;
        settingsManager.SetResolution(settingsManager.currentResolutionIndex);
    }

    private void OnEnable()
    {
        LoadSettingsToUI();
    }

    private void OnDisable()
    {
        SaveSettings();
    }

    private void OnApplicationQuit()
    {
        SaveSettings();
    }
}
