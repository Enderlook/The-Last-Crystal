using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    
    [Header("Setup")]

    [SerializeField, Tooltip("Toggles object.")]
    private TogglesUI[] toggles;

    [SerializeField, Tooltip("Child objects.")]
    private GameObject[] child;

    private bool isActiveTutorial = false;

    private static TutorialManager tutorialInstance;

    public static TutorialManager TutorialInstance
    {
        get
        {
            if (tutorialInstance == null)
            {
                GameObject tutorialManager = new GameObject();
            }

            return tutorialInstance;
        }
    }

    private void Awake() => tutorialInstance = this;

    private void Start() => InitializedToggles();

    private void Update()
    {
        if (isActiveTutorial)
            SetToggleByKeyCode();
    }

    private void SetToggleByKeyCode()
    {
        if (VerifyToggles())
        {
            DeactivateChildsObject();
            return;
        }
        for (int x = 0; x < toggles.Length; x++)
        {
            if (toggles[x].mouseButtons == TogglesUI.MouseButton.None)
            {
                for (int y = 0; y < toggles[x].keyCodes.Length; y++)
                {
                    if (Input.GetKeyDown(toggles[x].keyCodes[y]))
                    {
                        toggles[x].toggle.isOn = true;
                    }
                }
            }
            else if (toggles[x].mouseButtons != TogglesUI.MouseButton.None && 
                Input.GetMouseButtonDown((int)toggles[x].mouseButtons))
            {
                toggles[x].toggle.isOn = true;
            }
        }
    }

    private bool VerifyToggles()
    {
        bool isToggle = true;

        int x = 0;
        while (isToggle && x < toggles.Length)
        {
            if (toggles[x].toggle.isOn)
            {
                isToggle = true;
                x++;
            }
            else
                isToggle = false;
        }

        return isToggle ? true : false;
    }

    private void InitializedToggles()
    {
        for (int x = 0; x < toggles.Length; x++)
        {
            toggles[x].toggle.isOn = false;
        }
    }

    public void ActivateChildsObject()
    {
        isActiveTutorial = true;
        for (int i = 0; i < child.Length; i++)
        {
            child[i].SetActive(true);
        }
    }

    private void DeactivateChildsObject()
    {
        for (int i = 0; i < child.Length; i++)
        {
            child[i].SetActive(false);
        }
    }
}

[System.Serializable]
public class TogglesUI
{
    public enum MouseButton { None = -1, Left = 0, Right = 1, Middle = 2 }

    [Tooltip("Toggle object.")]
    public Toggle toggle;

    [Tooltip("Key action player.")]
    public KeyCode[] keyCodes;

    [Tooltip("Mouse action player.")]
    public MouseButton mouseButtons;
}
