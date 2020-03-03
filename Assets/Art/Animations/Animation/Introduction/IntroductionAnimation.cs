using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionAnimation : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField, Tooltip("Animator of the game object.")]
    private Animator animator;

    [SerializeField, Tooltip("State animation.")]
    private string state;

    [SerializeField, Tooltip("Script of menu.")]
    private Menu menu;

    [SerializeField, Tooltip("Level to go.")]
    private string levelName;

    public void PlayAnimation()
    {
        animator.SetBool(state, true);
    }

    public void GoToLevel()
    {
        animator.SetBool(state, false);
        menu.LoadScene(levelName);
    }
}
