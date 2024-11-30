using UnityEngine;
using DG.Tweening;

public class UISpinTween : MonoBehaviour
{
    [SerializeField] private float duration = 1f; // Duration for one full rotation
    [SerializeField] private RectTransform rectTransform; // The UI element to rotate

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        StartSpinning();
    }

    private void StartSpinning()
    {
        rectTransform
            .DORotate(new Vector3(0, 0, -360), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }
}
