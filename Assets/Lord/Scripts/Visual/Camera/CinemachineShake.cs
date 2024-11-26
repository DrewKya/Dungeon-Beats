using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake instance;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin multiChannelPerlin;

    private void Awake()
    {
        instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        multiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    }

    private void OnEnable()
    {
        instance = this;
    }

    public void ShakeCamera(float intensity, float duration)
    {
        StartCoroutine(CameraShakeCoroutine(intensity, duration));
    }

    private IEnumerator CameraShakeCoroutine(float intensity, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            multiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(intensity, 0f, time/duration);

            yield return null;
        }
        multiChannelPerlin.m_AmplitudeGain = 0f;
    }
}
