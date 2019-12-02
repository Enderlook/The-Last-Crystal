using System;
using UnityEngine;

public class Capturer : MonoBehaviour
{
    [SerializeField, Tooltip("Screenshoot key.")]
    private KeyCode capture;

    [SerializeField, Tooltip("Scale image resolution.")]
    private int resolutionMultiplier = 4;

    private void Start() => Screen.SetResolution(1920, 1080, true);

    private void Update()
    {
        if (Input.GetKeyDown(capture))
            ScreenCapture.CaptureScreenshot(Time.frameCount.ToString()+".png", resolutionMultiplier);
    }
}
