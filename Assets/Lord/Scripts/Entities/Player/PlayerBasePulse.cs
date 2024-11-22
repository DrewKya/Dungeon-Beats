using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasePulse : MonoBehaviour
{
    [SerializeField] private GameObject _base; // this is the indicator at the base of the player

    Vector3 initialScale;
    Vector3 pulseScale;

    private void Start()
    {
        initialScale = _base.transform.localScale;
        pulseScale = initialScale * 1.5f;

        MusicPlayer.Instance.interval.trigger.AddListener(Pulse);
    }

    private void Pulse()
    {
        StartCoroutine(PulseCoroutine());
    }

    private IEnumerator PulseCoroutine()
    {
        float duration = 0.2f;
        float elapsedTime = 0f;

        _base.transform.localScale = pulseScale;

        // Scale down
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            _base.transform.localScale = Vector3.Lerp(pulseScale, initialScale, t);
            yield return null;
        }

        _base.transform.localScale = initialScale; // Ensure it's set to the final scale
    }
}
