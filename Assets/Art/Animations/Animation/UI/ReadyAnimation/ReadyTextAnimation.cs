using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyTextAnimation : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField, Tooltip("True if you have a tutorial, False if you don't have a tutorial.")]
    private bool tutorial;

    public void InitializeEvent()
    {
        if (tutorial) TutorialManager.TutorialInstance.ActivateChildsObject();
        Spawner.Instance.InitializeWave();
    }
}
