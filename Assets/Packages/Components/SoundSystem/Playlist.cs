using UnityEngine;

namespace SoundSystem
{
    [CreateAssetMenu(fileName = "Playlist", menuName = "Playlist")]
    public class Playlist : ScriptableObject
    {
        [Tooltip("Name of the playlist, used to be access by other scripts.")]
        public string playlistName;

        [Tooltip("Playlist. It will be looped.")]
        public Sound[] playlist;

        private int playlistIndex = 0;

        [Tooltip("Playlist master volume.")]
        public float volume = 1;

        [Tooltip("Is the playlist random.")]
        public bool isRandom = false;

        /// <summary>
        /// Use <see cref="Random"/> to play sound by <seealso cref="GetRandomSound"/>.<br/>
        /// Use <see cref="Next"/> to play a sound by <seealso cref="GetNextSound"/>.<br/>
        /// Use <see cref="Configured"/> to play sound using <see cref="isRandom"/> to determine if use <seealso cref="GetRandomSound"/> or <seealso cref="GetNextSound"/>.
        /// </summary>
        public enum Mode { Random, Next, Configured }

        /// <summary>
        /// Get random sound from <see cref="playlist"/>.
        /// </summary>
        /// <returns>Sound to play and its playlist <see cref="volume"/>.</returns>
        public (Sound sound, float volume) GetRandomSound() => (playlist[playlistIndex = Random.Range(0, playlist.Length)], volume);

        /// <summary>
        /// Get the next sound from <see cref="playlist"/>. It loops to beginning when reach the end of the <see cref="playlist"/>.
        /// </summary>
        /// <returns>Sound to play and its playlist <see cref="volume"/>.</returns>
        public (Sound sound, float volume) GetNextSound() => (playlist[playlistIndex = (playlistIndex + 1) % playlist.Length], volume);

        /// <summary>
        /// Get a sound from <see cref="playlist"/>. It can be random or not depending of <see cref="isRandom"/>.
        /// </summary>
        /// <returns>Sound to play and its playlist <see cref="volume"/>.</returns>
        public (Sound sound, float volume) GetSound() => isRandom ? GetRandomSound() : GetNextSound();

        /// <summary>
        /// Reset the <see cref="playlistIndex"/> to 0.
        /// </summary>
        public void ResetIndex() => playlistIndex = 0;

        /// <summary>
        /// Play a sound from the <seealso cref="playlist"/> using a method described by <seealso cref="Mode"/>.
        /// </summary>
        /// <param name="audioSource"><see cref="AudioSource"/> to play the sonud.</param>
        /// <param name="isSoundActive">Whenever sound is active or not. On <see langword="false"/> no sound will be played.</param>
        /// <param name="mode">Mode to get the sound form the <seealso cref="playlist"/>.</param>
        /// <param name="volumeMultiplier">Multiplier of the volume.</param>
        public void Play(AudioSource audioSource, bool isSoundActive, Mode mode = Mode.Configured, float volumeMultiplier = 1) => Play(audioSource, isSoundActive, IsRandomFromMode(mode), volumeMultiplier);

        /// <summary>
        /// Play a sound from the <seealso cref="playlist"/>
        /// </summary>
        /// <param name="audioSource"><see cref="AudioSource"/> to play the sound.</param>
        /// <param name="isSoundActive">Whenever sound is active or not. On <see langword="false"/> no sound will be played.</param>
        /// <param name="isRandom">If <see langword="true"/> a random sound from <see cref="playlist"/> will be played. On <see langword="false"/> the next sound from the <see cref="playlist"/> will be played.</param>
        /// <param name="volumeMultiplier">Multiplier of the volume.</param>
        public void Play(AudioSource audioSource, bool isSoundActive, bool isRandom, float volumeMultiplier = 1)
        {
            (Sound sound, float volume) = GetSound(isRandom);
            sound.Play(audioSource, isSoundActive, volume * volumeMultiplier);
        }

        /// <summary>
        /// Play a sound from the <seealso cref="playlist"/> using a method described by <seealso cref="Mode"/> on the specified <paramref name="position"/>.
        /// </summary>
        /// <param name="position">Position to play the sound.</param>
        /// <param name="isSoundActive">Whenever sound is active or not. On <see langword="false"/> no sound will be played.</param>
        /// <param name="mode">Mode to get the sound form the <seealso cref="playlist"/>.</param>
        /// <param name="volumeMultiplier">Multiplier of the volume.</param>
        public void PlayAtPoint(Vector3 position, bool isSoundActive, Mode mode = Mode.Configured, float volumeMultiplier = 1) => PlayAtPoint(position, isSoundActive, IsRandomFromMode(mode), volumeMultiplier);

        /// <summary>
        /// Play a sound from the <seealso cref="playlist"/> using a method described by <seealso cref="Mode"/> on the specified <paramref name="position"/>.
        /// </summary>
        /// <param name="position">Position to play the sound.</param>
        /// <param name="isSoundActive">Whenever sound is active or not. On <see langword="false"/> no sound will be played.</param>
        /// <param name="isRandom">If <see langword="true"/> a random sound from <see cref="playlist"/> will be played. On <see langword="false"/> the next sound from the <see cref="playlist"/> will be played.</param>
        /// <param name="volumeMultiplier">Multiplier of the volume.</param>
        public void PlayAtPoint(Vector3 position, bool isSoundActive, bool isRandom, float volumeMultiplier = 1)
        {
            if (isSoundActive)
            {
                (Sound sound, float volume) = GetSound(isRandom);
                AudioSource.PlayClipAtPoint(sound.audioClip, position, sound.Volume * volume * volumeMultiplier);
            }
        }

        private (Sound sound, float volume) GetSound(bool isRandom) => isRandom ? GetRandomSound() : GetNextSound();

        private bool IsRandomFromMode(Mode mode = Mode.Configured) => (mode == Mode.Configured && isRandom) || mode == Mode.Random;
    }
}