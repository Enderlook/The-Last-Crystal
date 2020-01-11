using AdditionalExceptions;

using Master;

using System;

using UnityEngine;

namespace SoundSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class PlaylistManager : MonoBehaviour
    {
#pragma warning disable CS0649
        [Header("Configuration")]
        [SerializeField, Tooltip("Playlists.")]
        private Playlist[] playlists;

        [SerializeField, Tooltip("Default playlist set at start.")]
        private int startingPlaylistIndex;

        private int playlistsIndex;

        [SerializeField, Range(0f, 1f), Tooltip("Master volume.")]
        private float masterVolume = 1;

        [SerializeField, Tooltip("Play on start.")]
        private bool playOnStart;

        [Header("Setup")]
        [Tooltip("Audio Source used to play music.")]
#pragma warning disable IDE0044
        private AudioSource audioSource;

        [Tooltip("How is this considered.")]
        private Type type;
#pragma warning restore IDE0044
#pragma warning restore CS0649

        /// <summary>
        /// Determines how it will be treated.
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// It will use <see cref="Settings.IsMusicActive"/> to check if should play.
            /// </summary>
            Music,

            /// <summary>
            /// It will use <see cref="Settings.IsSoundActive"/> to check if should play.
            /// </summary>
            Sound
        }

        private bool isPlaying;

        private Func<bool> shouldPlay;
        private bool ShouldPlay => shouldPlay != null ? shouldPlay() : throw new InvalidOperationException("Start method must be call first.");

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0066:Convert switch statement to expression", Justification = "Not supported by Unity.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Start()
        {
            audioSource.loop = false;
            isPlaying = playOnStart;
            playlistsIndex = startingPlaylistIndex;

            switch (type)
            {
                case Type.Music:
                    shouldPlay = () => Settings.IsMusicActive;
                    break;
                case Type.Sound:
                    shouldPlay = () => Settings.IsSoundActive;
                    break;
                default:
                    throw new ImpossibleStateException();
            }
        }

        public void Update()
        {
            if (!ShouldPlay)
                audioSource.Stop();
            else if (isPlaying // We should play
                && !audioSource.isPlaying // But we aren't currently playing
                && playlists.Length > 0 // And we have playlists to play
                && playlists[playlistsIndex].PlaylistLength > 0) // And the current playlist has sounds inside
                playlists[playlistsIndex].Play(audioSource, volumeMultiplier: masterVolume);
        }

        /// <summary>
        /// Stop current play and play an <paramref name="audioClip"/>.
        /// </summary>
        /// <param name="audioClip"><see cref="AudioClip"/> to play.</param>
        /// <param name="volume">Volume of sound, from 0 to 1.</param>
        /// <param name="pitch">Pitch of sound.</param>
        /// <param name="useMasterVolumeMultplier">If <see langword="true"/> <paramref name="volume"/> will be multiplied by <see cref="masterVolume"/>.</param>
        public void PlaySound(AudioClip audioClip, float volume = 1, float pitch = 1, bool useMasterVolumeMultplier = false)
        {
            if (audioClip)
                throw new ArgumentNullException(nameof(audioClip));
            if (volume < 0 && volume > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volume));

            audioSource.Stop();
            audioSource.clip = audioClip;
            audioSource.volume = useMasterVolumeMultplier ? volume * masterVolume : volume;
            audioSource.pitch = pitch;
            audioSource.Play();
        }

        /// <summary>
        /// Stop current play and play an <paramref name="audioClip"/>.
        /// </summary>
        /// <param name="audioClip"><see cref="AudioClip"/> to play.</param>
        /// <param name="useMasterVolumeMultplier">If <see langword="true"/> it will use <see cref="masterVolume"/>.</param>
        public void PlaySound(AudioClip audioClip, bool useMasterVolumeMultplier = false)
        {
            if (audioClip)
                throw new ArgumentNullException(nameof(audioClip));

            PlaySound(audioClip, 1, 1, useMasterVolumeMultplier);
        }

        /// <summary>
        /// Stop current play and play a <paramref name="sound"/>.
        /// </summary>
        /// <param name="sound">Sound to play.</param>
        /// <param name="useMasterVolumeMultplier">If <see langword="true"/> <paramref name="volume"/> will be multiplied by <see cref="masterVolume"/>.</param>
        public void PlaySound(Sound sound, bool useMasterVolumeMultplier = false)
        {
            if (sound == null)
                throw new ArgumentNullException(nameof(sound));

            audioSource.Stop();
            sound.Play(audioSource, useMasterVolumeMultplier ? masterVolume : 1);
        }

        /// <summary>
        /// Play a <paramref name="audioClip"/>.
        /// </summary>
        /// <param name="sound">Sound to play.</param>
        /// <param name="volumeMultiplier">Additional volume multiplier.</param>
        /// <param name="useMasterVolumeMultplier">If <see langword="true"/> <paramref name="volume"/> will be multiplied by <see cref="masterVolume"/>.</param>
        public void PlaySound(Sound sound, float volumeMultiplier = 1, bool useMasterVolumeMultplier = false)
        {
            audioSource.Stop();
            sound.Play(audioSource, useMasterVolumeMultplier ? volumeMultiplier * masterVolume : volumeMultiplier);
        }

        /// <summary>
        /// Set a playlist. It doesn't reset current playing sound.
        /// </summary>
        /// <param name="index">Index of the playlist.</param>
        /// <returns><see langword="true"/> on success. <see langword="false"/> if the <paramref name="index"/> was outside the range of the <see cref="playlists"/>.</returns>
        public bool SetPlaylist(int index)
        {
            if (index >= playlists.Length)
                return false;
            playlistsIndex = index;
            return true;
        }

        /// <summary>
        /// Set a playlist. It doesn't reset current playing sound.
        /// </summary>
        /// <param name="name">Name of the playlist.</param>
        /// <returns><see langword="true"/> on success. <see langword="false"/> if the <paramref name="name"/> was not found in <see cref="playlists"/>.</returns>
        public bool SetPlaylist(string name)
        {
            for (int i = 0; i < playlists.Length; i++)
            {
                if (playlists[i].PlayListName == name)
                {
                    playlistsIndex = i;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Play or resume the sound.
        /// </summary>
        public void Play()
        {
            isPlaying = true;
            audioSource.UnPause();
        }

        /// <summary>
        /// Pauses the played sound.
        /// </summary>
        public void Pause()
        {
            isPlaying = false;
            audioSource.Pause();
        }

        /// <summary>
        /// Stop the played sound.
        /// </summary>
        public void Stop()
        {
            isPlaying = false;
            audioSource.Stop();
        }

        /// <summary>
        /// Reset the playlist to the first sound.
        /// <paramref name="stopCurrentSound"/>If <see langword="true"/>, execute <see cref="StopCurrentSound"/> will be called.<paramref name="stopCurrentSound"/>
        /// </summary>
        public void ResetPlaylist(bool stopCurrentSound)
        {
            playlists[playlistsIndex].ResetIndex();
            if (stopCurrentSound)
                StopCurrentSound();
        }

        /// <summary>
        /// Stop current sound.
        /// </summary>
        public void StopCurrentSound() => audioSource.Stop();

#if UNITY_EDITOR
        private readonly string STARTING_PLAYLIST_INDEX_ERROR = $"{nameof(startingPlaylistIndex)} must be an index of {nameof(playlists)}. So it must be greater than 0 and lower than {nameof(playlists)}.length";

        private void OnValidate()
        {
            if (startingPlaylistIndex >= playlists.Length)
            {
                Debug.LogError(STARTING_PLAYLIST_INDEX_ERROR);
                startingPlaylistIndex = playlists.Length - 1;
            }
            else if (startingPlaylistIndex < 0)
            {
                Debug.LogError(STARTING_PLAYLIST_INDEX_ERROR);
                startingPlaylistIndex = 0;
            }
        }
#endif
    }
}
