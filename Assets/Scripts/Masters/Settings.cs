﻿using UnityEngine;
using UnityEngine.SceneManagement;


public class Settings : MonoBehaviour
{
    /// <summary>
    /// Whenever sound is mute or not.
    /// </summary>
    public static bool IsSoundActive = true;
    /// <summary>
    /// Whenever music is mute or not.
    /// </summary>
    public static bool IsMusicActive = true;

    /// <summary>
    /// Set if the sound is played or muted.
    /// </summary>
    /// <param name="active">If <see langword="true"/> sound will be played. On <see langword="false"/> sound is muted.</param>
    public void SetSound(bool active) => IsSoundActive = active;

    /// <summary>
    /// Set if the music is played or muted.
    /// </summary>
    /// <param name="active">If <see langword="true"/> music will be played. On <see langword="false"/> music is muted.</param>
    public void SetMusic(bool active) => IsMusicActive = active;

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
    /// Load an scene.\n
    /// Equivalent to <c><seealso cref="SceneManager.LoadScene"/>(<paramref name="scene"/>, <seealso cref="LoadSceneMode.Single"/>);</c>
    /// </summary>
    /// <seealso cref="SceneManager.LoadScene(string)"/>
    /// <param name="scene">Scene name to load.</param>
    private void LoadScene(string scene) => SceneManager.LoadScene(scene, LoadSceneMode.Single);
}
