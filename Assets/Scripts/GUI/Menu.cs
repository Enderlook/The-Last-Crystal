using Additions.Components.SoundSystem;

using Master;

using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
#pragma warning disable CS0649
    [Header("Setup")]
    [SerializeField, Tooltip("Menu to display on escape key press.")]
    private GameObject menu;

    [SerializeField, Tooltip("Others panels. Used to hide them when press escape.")]
    private GameObject[] panels;

    [SerializeField, Tooltip("Force menu to be untoggleable.")]
    private bool menuNoToggleable;

    [SerializeField, Tooltip("Playlist Manager.")]
    private PlaylistManager playlistManager;

    [SerializeField, Tooltip("Name of the playlist to play when menu is shown.")]
    private string playlistMenuShow;

    [SerializeField, Tooltip("Name of the playlist to play when menu is hide.")]
    private string playlistMenuHide;

    [SerializeField, Tooltip("Panel displayed on win.")]
    private GameObject win;

    [SerializeField, Tooltip("Panel displayed on defeat.")]
    private GameObject lose;

    [Header("Animation")]
    [SerializeField, Tooltip("Animator component.")]
    private Animator animator;

    private string sceneToLoad;

    private static class ANIMATION_STATES
    {
        public const string
            FADEOUT = "FadeOut";
    }
#pragma warning disable CS0649

    private void Start() => DisplayMenuPause(false);

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
    /// <param name="continueWhenHideLastPanel">When the last panel from <see cref="panels"/> is hidden it determines if <see cref="menu"/> should be shown even when it's hidden or <see cref="menuNoToggleable"/> is <see langword="true"/>.</param>
    public void DisplayMenuPause(bool? active = null, bool continueWhenHideLastPanel = false)
    {
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
            {
                panel.SetActive(false);
                if (continueWhenHideLastPanel)
                    break;
                else
                    return;
            }
        }
        if (menuNoToggleable)
        {
            if (continueWhenHideLastPanel)
                menu.SetActive(true);
            return;
        }
        Settings.IsPause = active == null ? !Settings.IsPause : (bool)active;
        menu.SetActive(Settings.IsPause);
        PlayMusic(Settings.IsPause, true);
    }

    /// <summary>
    /// Hide the menu and set to <see langword="false"/> <seealso cref="IsPause"/>.
    /// </summary>
    public void HideMenu() => DisplayMenuPause(false);

    /// <summary>
    /// Show the menu and set to <see langword="true"/> <seealso cref="IsPause"/>.
    /// </summary>
    public void GoToMenu() => DisplayMenuPause(true, true);

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
    public void LoadScene(string scene)
    {
        sceneToLoad = scene;
        animator.SetTrigger(ANIMATION_STATES.FADEOUT);
    }

    /// <summary>
    /// Load a scene through the animation event.
    /// </summary>
    public void FadeAnimationToLevel() => SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);

    /// <summary>
    /// End game.
    /// </summary>
    /// <param name="hasWon">Whenever players has won or loose.</param>
    public void GameOver(bool hasWon)
    {
        Settings.IsPause = true;
        menuNoToggleable = true;
        if (hasWon)
            win.SetActive(true);
        else
            lose.SetActive(true);
    }
#pragma warning restore CA1822 // Unity Editor can't assign static methods to buttons
}