using UnityEngine;
using UnityEngine.UI;

public class SliderCount : MonoBehaviour
{
    private Text text;

    private void Awake() => text = GetComponent<Text>();

    public void Change(float newText)
    {
        // Not exactly sure why but it seems like sliders can call this before awake is called
        // That is why we check for null
        if (text == null)
            text = GetComponent<Text>();
        text.text = Mathf.Round(newText * 100).ToString() + '%';
    }
}
