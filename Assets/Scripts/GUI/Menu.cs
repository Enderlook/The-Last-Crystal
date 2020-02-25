
using Additions.Components.ScriptableSound;
using Additions.Serializables.PolySwitcher;
using Master;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [SerializeField, Tooltip("Show slider values in UI.")]
    private ValueSlider[] valueSliders;

    [SerializeField, Tooltip("Resolution dropdown.")]
    private Dropdown resolutionDropdown;

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

    [SerializeField, Tooltip("Difficulty.")]
    private PolySwitchMaster difficulty;

    [SerializeField, Tooltip("Difficulty text.")]
    private Text difficultyText;

    [Header("Animation")]
    [SerializeField, Tooltip("Animator component.")]
    private Animator animator;

    private Resolution[] resolutions; // Variable to store all detected resolutions.

    // Statics variables.
    private static int currentResolutionIndex;

    private static class ANIMATIONS
    {
        public const string
            SHOW_INTRO = "ShowIntro",
            SHOW_PRESS_ANY_KEY = "ShowPressAnyKey";
    }

#pragma warning disable CS0649

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
    private void Start()
    {
        SetResolutionsInDropdown();
        DisplayMenuPause(false);
        ShowDifficulty();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            DisplayMenuPause();
    }

    private void SetResolutionsInDropdown()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> optionsDropDown = new List<string>();

        for (int x = 0; x < resolutions.Length; x++)
        {
            string option = $"{resolutions[x].width} x {resolutions[x].height}";
            optionsDropDown.Add(option);

            currentResolutionIndex = resolutions[x].width == Screen.currentResolution.width 
                && resolutions[x].height == Screen.currentResolution.height ? x : currentResolutionIndex;
        }

        resolutionDropdown.AddOptions(optionsDropDown);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    /// <summary>
    /// Shift current difficulty by one and update difficulty button text.
    /// </summary>
    public void ChangeDifficulty()
    {
        difficulty.IncrementIndexByOne();
        ShowDifficulty();
    }

    private void ShowDifficulty()
    {
        switch (difficulty.CurrentIndex)
        {
            case 1:
                difficultyText.text = "EASY";
                break;
            case 2:
                difficultyText.text = "NORMAL";
                break;
            case 3:
                difficultyText.text = "HARD";
                break;
        }
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

    /// <summary>
    /// Animation state to show screen intro, called through event.
    /// </summary>
    public void ShowSplashIntro() => animator.SetTrigger(ANIMATIONS.SHOW_INTRO);

    /// <summary>
    /// Animation state to show text in screen intro, called through event.
    /// </summary>
    public void ShowPressAnyKeyText() => animator.SetTrigger(ANIMATIONS.SHOW_PRESS_ANY_KEY);

    /// <summary>
    /// Function called through the sliders event.
    /// </summary>
    /// <param name="index">Array index.</param>
    public void TextUpdate(int index = 0) => valueSliders[index].valueSliderText.text = $"{valueSliders[index].slider.value}";

    /// <summary>
    /// Set the quality of the game.
    /// </summary>
    /// <param name="index">Array index.</param>
    public void SetQuality(int index) => QualitySettings.SetQualityLevel(index);

    /// <summary>
    /// Set the fullscreen mode.
    /// </summary>
    /// <param name="isFullScreen"><seealso cref="bool"/> value to change fullscreen mode.</param>
    public void SetScreenMode(bool isFullScreen) => Screen.fullScreen = isFullScreen;

    /// <summary>
    /// Set the resolution of the game.
    /// </summary>
    /// <param name="index">Array index</param>
    public void SetResolution(int index)
    {
        currentResolutionIndex = index;
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
    }

#pragma warning restore CA1822 // Unity Editor can't assign static methods to buttons
}

[System.Serializable]
public class ValueSlider
{
    [Tooltip("Name of the slider value.")]
    public string nameSlider;

    [Tooltip("Text to show slider values.")]
    public Text valueSliderText;

    [Tooltip("Slider.")]
    public Slider slider;
}