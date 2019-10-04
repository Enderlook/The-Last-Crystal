using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("Menu to display on escape key press.")]
    public GameObject menu;
    [Tooltip("Others panels. Used to hide them when press escape.")]
    public GameObject[] panels;
    [Tooltip("Force menu to be untoggleable.")]
    public bool menuNoToggleable = false;
    [Tooltip("Playlist Manager.")]
    public PlaylistManager playlistManager;
    [Tooltip("Name of the playlist to play when menu is shown.")]
    public string playlistMenuShow;
    [Tooltip("Name of the playlist to play when menu is hide.")]
    public string playlistMenuHide;

    private static List<Animator> animationsToRenable = new List<Animator>();
    private static Dictionary<StoppableRigidbody, float> stoppableRigidbodySpeeds = new Dictionary<StoppableRigidbody, float>();

    public static bool IsPlaying => !isPause;
    private static bool isPause = false;
    public static bool IsPause {
        get => isPause;
        private set {
            if (isPause == value)
                return;
            isPause = value;
            if (value)
            {
                foreach (Animator animator in FindObjectsOfType<Animator>())
                {
                    if (animator.enabled == true)
                    {
                        animator.enabled = false;
                        animationsToRenable.Add(animator);
                    }
                }

                foreach (StoppableRigidbody stoppableRigidbody in FindObjectsOfType<StoppableRigidbody>())
                {
                    stoppableRigidbodySpeeds.Add(stoppableRigidbody, stoppableRigidbody.SpeedMultiplier);
                    stoppableRigidbody.SpeedMultiplier = 0;
                }
            }
            else
            {
                animationsToRenable.ForEach(e => e.enabled = true);
                animationsToRenable.Clear();
                foreach (KeyValuePair<StoppableRigidbody, float> keyValuePair in stoppableRigidbodySpeeds)
                {
                    keyValuePair.Key.SpeedMultiplier = keyValuePair.Value;
                }
                stoppableRigidbodySpeeds.Clear();
            }
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Calidad del código", "IDE0051:Quitar miembros privados no utilizados", Justification = "Used by Unity.")]
    private void Start() => DisplayMenuPause(false);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Calidad del código", "IDE0051:Quitar miembros privados no utilizados", Justification = "Used by Unity.")]
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            DisplayMenuPause();
    }

    /// <summary>
    /// Toggle visibility of the <see cref="menu"/> using the opposite value of <seealso cref="IsPause"/>. <seealso cref="IsPause"/> is set as its opposite value.<br/>
    /// If <paramref name="active"/> isn't null this value will override the toggle.<br/>
    /// Panels inside <seealso cref="panels"/> will be hidden first, one by one. If all of them are hidden, menu will hide.
    /// </summary>
    /// <param name="active">Whenever the menu is visible or not.</param>
    public void DisplayMenuPause(bool? active = null)
    {
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
            {
                panel.SetActive(false);
                return;
            }
        }
        if (menuNoToggleable)
            return;
        IsPause = active != null ? (bool)active : !IsPause;
        Time.timeScale = IsPause ? 0 : 1;
        menu.SetActive(IsPause);
        PlayMusic(IsPause, true);
    }

    /// <summary>
    /// Hide the menu and set to <see langword="false"/> <seealso cref="IsPause"/>.
    /// </summary>
    public void HideMenu() => DisplayMenuPause(false);

    /// <summary>
    /// Play music.
    /// </summary>
    /// <param name="menuMusic">Whenever menu music should be player or game music.</param>
    /// <param name="resetCurrentMusic">Whenever it should reset the current music (not playlist) or wait until it ends.</param>
    public void PlayMusic(bool menuMusic, bool resetCurrentMusic)
    {
        if (playlistManager == null)
            return;
        if (menuMusic)
            playlistManager.SetPlaylist(playlistMenuShow);
        else
            playlistManager.SetPlaylist(playlistMenuHide);
        playlistManager.ResetPlaylist(resetCurrentMusic);
    }

#pragma warning disable CA1822 // Unity Editor can't assign static methods to buttons
    /// <summary>
    /// Close game.
    /// </summary>
    public void Exit() => Application.Quit();

    /// <summary>
    /// Reload the current scene.
    /// </summary>
    public void Restart() => LoadScene(SceneManager.GetActiveScene().name);

    /// <summary>
    /// Load the main menu scene.
    /// </summary>
    public void MainMenu() => LoadScene("Main_Menu");

    /// <summary>
    /// Load an scene.<br>
    /// Equivalent to <c><seealso cref="SceneManager.LoadScene"/>(<paramref name="scene"/>, <seealso cref="LoadSceneMode.Single"/>);</c>
    /// </summary>
    /// <seealso cref="SceneManager.LoadScene(string)"/>
    /// <param name="scene">Scene name to load.</param>
    public void LoadScene(string scene) => SceneManager.LoadScene(scene, LoadSceneMode.Single);
#pragma warning restore CA1822 // Unity Editor can't assign static methods to buttons
}