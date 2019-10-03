using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("Menu to display on escape key press.")]
    public GameObject menu;
    [Tooltip("Others panels. Used to hide them when press escape.")]
    public GameObject[] panels;
    [Tooltip("Force menu to not be toggleable.")]
    public bool menuNoToggleable = false;
    [Tooltip("Playlist Manager.")]
    public PlaylistManager playlistManager;
    [Tooltip("Name of the playlist to play when menu is shown")]
    public string playlistMenuShow;
    [Tooltip("Name of the playlist to play when menu is hide")]
    public string playlistMenuHide;

    public bool IsPause { get; private set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Calidad del c�digo", "IDE0051:Quitar miembros privados no utilizados", Justification = "Used by Unity.")]
    private void Start() => DisplayMenuPause(false);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Calidad del c�digo", "IDE0051:Quitar miembros privados no utilizados", Justification = "Used by Unity.")]
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
        if (menuMusic)
            playlistManager.SetPlaylist(playlistMenuShow);
        else
            playlistManager.SetPlaylist(playlistMenuHide);
        playlistManager.ResetPlaylist(resetCurrentMusic);
    }
}