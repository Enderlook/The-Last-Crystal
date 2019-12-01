using UnityEngine;

public class Capturer : MonoBehaviour
{
    [SerializeField, Tooltip("Screenshoot key.")]
    private KeyCode capture;

    private void Start() => Screen.SetResolution(1920, 1080, true);

    private void Update()
    {
        if (Input.GetKeyDown(capture))
            ScreenCapture.CaptureScreenshot(Time.frameCount.ToString()+".png", 4);
    }
}
