
using Additions.Components.ScriptableSound;

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

    [SerializeField, Tooltip("Panel displayed on win.")]
    private GameObject win;

    [SerializeField, Tooltip("Panel displayed on defeat.")]
    private GameObject lose;

    [Header("Music")]
    [SerializeField, Tooltip("Sound player script which manages music.")]
    private SoundPlayer soundPlayer;

    [SerializeField, Tooltip("Sound index play on game.")]
    private int gameIndex;

    [SerializeField, Tooltip("Sound index play on menu.")]
    private int menuIndex;

    [SerializeField, Tooltip("Sound index play on win.")]
    private int winIndex;

    [SerializeField, Tooltip("Sound index play on lose.")]
    private int loseIndex;

    [Header("Animation")]
    [SerializeField, Tooltip("Animator component.")]
    private Animator animator;

#pragma warning disable CS0649

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
    private void Start() => DisplayMenuPause(false);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
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
        if (soundPlayer != null)
            soundPlayer.Play(Settings.IsPause ? menuIndex : gameIndex);
    }

    /// <summary>
    /// Hide the menu and set to <see langword="false"/> <seealso cref="IsPause"/>.
    /// </summary>
    public void HideMenu() => DisplayMenuPause(false);

    /// <summary>
    /// Show the menu and set to <see langword="true"/> <seealso cref="IsPause"/>.
    /// </summary>
    public void GoToMenu() => DisplayMenuPause(true, true);

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

    /// <summary>
    /// End game.
    /// </summary>
    /// <param name="hasWon">Whenever players has won or loose.</param>
    public void GameOver(bool hasWon)
    {
        Settings.IsPause = true;
        menuNoToggleable = true;
        if (hasWon)
        {
            win.SetActive(true);
            if (soundPlayer != null)
                soundPlayer.Play(winIndex);
        }
        else
        {
            lose.SetActive(true);
            if (soundPlayer != null)
                soundPlayer.Play(loseIndex);
        }
    }
#pragma warning restore CA1822 // Unity Editor can't assign static methods to buttons
}