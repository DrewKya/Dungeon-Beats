using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script determines the currentCamera from the last enabled camera
// add this script to every camera

[RequireComponent(typeof(Camera))]
public class CameraTracker : MonoBehaviour
{
    private static Camera currentCamera;
    public static Camera CurrentCamera => currentCamera;

    void OnEnable()
    {
        currentCamera = GetComponent<Camera>();
    }
}
