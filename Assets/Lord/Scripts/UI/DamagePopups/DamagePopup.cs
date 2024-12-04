using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Pool;

public class DamagePopup : MonoBehaviour
{
    TMP_Text textMesh;
    Camera currentCamera;

    [SerializeField] private float popupDuration = 2f;
    [SerializeField] private float moveDistance = 1f; // Distance to move upward
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
        Camera.onPreRender += UpdateCurrentCamera;
    }

    private void Start()
    {
        if (currentCamera == null) currentCamera = Camera.main;
    }

    private void OnDestroy()
    {
        Camera.onPreRender -= UpdateCurrentCamera;
    }

    private void UpdateCurrentCamera(Camera camera)
    {
        Debug.Log($"Camera.onPreRender called: {camera.name}");
        currentCamera = camera;
    }

    public void SetDamage(int damage, bool isCrit, Camera currentCamera)
    {
        if (textMesh == null) textMesh = GetComponent<TMP_Text>();


        if (!isCrit)
        {
            textMesh.text = damage.ToString();
            textMesh.fontStyle = FontStyles.Normal;
            textMesh.fontSize = 36;
        }
        else
        {
            textMesh.text = damage.ToString() + "!";
            textMesh.fontStyle = FontStyles.Bold;
            textMesh.fontSize = 50;
        }

        transform.LookAt(transform.position + currentCamera.transform.rotation * Vector3.forward, currentCamera.transform.rotation * Vector3.up);

        PlayPopupAnimation();
    }

    private void PlayPopupAnimation()
    {
        var color = textMesh.color;
        color.a = 1f; // Set alpha back to fully visible
        textMesh.color = color;

        // Move upwards
        transform.DOMoveY(transform.position.y + moveDistance, popupDuration).SetEase(Ease.OutQuad);

        // Fade out
        textMesh.DOFade(0, fadeDuration).SetDelay(popupDuration - fadeDuration).OnComplete(() =>
        {
            gameObject.SetActive(false); // Deactivate after animation
        });
    }
}