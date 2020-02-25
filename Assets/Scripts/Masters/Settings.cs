using UnityEngine;
using UnityEngine.SceneManagement;

namespace Master
{
    public class Settings : MonoBehaviour
    {
        private static bool isSoundActive = true;
        /// <summary>
        /// Whenever sound is mute or not.
        /// </summary>
        public static bool IsSoundActive => isSoundActive && (MasterVolume * SoundVolume) > 0;

        private static bool isMusicActive = true;
        /// <summary>
        /// Whenever music is mute or not.
        /// </summary>
        public static bool IsMusicActive => isMusicActive && (MasterVolume * MusicVolume) > 0;

        /// <summary>
        /// Set if the sound is played or muted.
        /// </summary>
        /// <param name="active">If <see langword="true"/> sound will be played. On <see langword="false"/> sound is muted.</param>
        public void SetSound(bool active) => isSoundActive = active;

        /// <summary>
        /// Set if the music is played or muted.
        /// </summary>
        /// <param name="active">If <see langword="true"/> music will be played. On <see langword="false"/> music is muted.</param>
        public void SetMusic(bool active) => isMusicActive = active;

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

        /// <summary>
        /// Whenever the game is playing or not.<br>
        /// If possible, use <seealso cref="IsPause"/>.
        /// </summary>
        public static bool IsPlaying => !isPause;

        private static bool isPause;

        /// <summary>
        /// Whenever the game is paused or not.
        /// </summary>
        public static bool IsPause {
            get => isPause;
            set {
                isPause = value;
                Time.timeScale = isPause ? 0 : 1;
            }
        }

        /// <summary>
        /// Control master game sound.
        /// </summary>
        public static float MasterVolume {
            get => AudioListener.volume;
            set => AudioListener.volume = value;
        }

        /// <summary>
        /// Control sound volume.
        /// </summary>
        public static float SoundVolume;

        /// <summary>
        /// Control music volume.
        /// </summary>
        public static float MusicVolume;
    }
}