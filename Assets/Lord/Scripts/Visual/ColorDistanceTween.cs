using UnityEngine;
using UnityEngine.UI;

public class ColorDistanceTween : MonoBehaviour
{
    public RectTransform targetObject;

    public Color farColor = Color.red;
    public Color closeColor = Color.blue;
    public float thresholdDistance = 100f; // Distance threshold for full interpolation
    public float minDistance = 0f; // Minimum distance where color is fully 'closeColor'

    private RectTransform rectTransform;
    private Image imageToTween;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        imageToTween = GetComponent<Image>();
    }

    void FixedUpdate()
    {
        float distance = Vector3.Distance(rectTransform.position, targetObject.position);

        // Interpolate the distance to a value between 0 and 1
        float t = Mathf.InverseLerp(minDistance, thresholdDistance, distance);

        Color targetColor = Color.Lerp(farColor, closeColor, t);

        imageToTween.color = targetColor;
    }
}
