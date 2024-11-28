using UnityEngine;
using UnityEngine.UI;

public class ColorDistanceTween : MonoBehaviour
{
    public RectTransform targetObject;

    public float nearThreshold = 50f; // Distance where transition is fully color1
    public float farThreshold = 300f; // Distance where transition is fully color2

    private RectTransform rectTransform;
    private Image imageToTween;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        imageToTween = GetComponent<Image>();

        Material material = new Material(imageToTween.material);
        imageToTween.material = material;
    }

    void FixedUpdate()
    {
        float distance = Vector3.Distance(rectTransform.position, targetObject.position);

        // Interpolate the distance to a value between 0 and 1
        float t = Mathf.InverseLerp(farThreshold, nearThreshold, distance);

        // Set the _Transition value in the material
        imageToTween.material.SetFloat("_Transition", t);
    }
}
