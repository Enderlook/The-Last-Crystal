using System;
using UnityEngine;
using AdditionalExceptions;
using Random = UnityEngine.Random;

namespace SoundSystem
{
    [CreateAssetMenu(fileName = "Playlist", menuName = "Playlist")]
    public class Playlist : ScriptableObject
    {
        [SerializeField, Tooltip("Name of playlist, used to be access by other scripts.")]
#pragma warning disable CS0649
        private string playlistName;
#pragma warning restore CS0649
        public string PlayListName => playlistName;

        [SerializeField, Tooltip("Playlist.")]
#pragma warning disable CS0649
        private Sound[] playlist;
#pragma warning restore CS0649
        private int playlistIndex;
        private bool foward = true;
        public int PlaylistLength => playlist.Length;

        [SerializeField, Range(0f, 1f), Tooltip("Playlist master volume.")]
        private float volume = 1;
        public float Volume => volume;

        [SerializeField, Tooltip("Playmode.")]
#pragma warning disable CS0649
        private PlayMode playingMode;
#pragma warning restore CS0649
        public PlayMode PlayingMode => playingMode;

        /// <summary>
        /// Use <see cref="Random"/> to play sound by <seealso cref="GetRandomSound"/>.<br/>
        /// Use <see cref="Next"/> to play a sound by <seealso cref="GetNextSound"/>.<br/>
        /// Use <see cref="Configured"/> to play sound using <see cref="isRandom"/> to determine if use <seealso cref="GetRandomSound"/> or <seealso cref="GetNextSound"/>.
        /// Playmodes.
        /// </summary>
        public enum Mode { Random, Next, Configured }
        public enum PlayMode
        {
            /// <summary>
            /// Display form first to last and them go back to beginning.
            /// </summary>
            Next,

            /// <summary>
            /// Display form first to last and from last to first.
            /// </summary>
            PingPong,

            /// <summary>
            /// Display randomly.
            /// </summary>
            Random
        };

        /// <summary>
        /// Get random sound from <see cref="playlist"/>.<br/>
        /// Don't forget to use <see cref="Volume"/> for this <see cref="Playlist"/> master volume.
        /// </summary>
        /// <returns>Sound to play.</returns>
        public Sound GetRandomSound() => playlist[playlistIndex = Random.Range(0, playlist.Length)];

        /// <summary>
        /// Get the next sound from <see cref="playlist"/>.<br/>
        /// It loops to beginning when reach the end of the <see cref="playlist"/>.<br/>
        /// Don't forget to use <see cref="Volume"/> for this <see cref="Playlist"/> master volume.
        /// </summary>
        /// <returns>Sound to play and its playlist <see cref="volume"/>.</returns>
        public Sound GetNextSound() => playlist[playlistIndex = (playlistIndex + 1) % playlist.Length];

        /// <summary>
        /// Get the next sound from <see cref="playlist"/>.<br/>
        /// It plays in reverse order when reach the end of the <see cref="playlist"/>.
        /// Don't forget to use <see cref="Volume"/> for this <see cref="Playlist"/> master volume.
        /// </summary>
        /// <returns>Sound to play and its playlist <see cref="volume"/>.</returns>
        public Sound GetPingPongSound()
        {
            if (foward)
            {
                playlistIndex++;
                if (playlistIndex == playlist.Length - 1)
                    foward = false;
            }
            else
            {
                playlistIndex--;
                if (playlistIndex == 0)
                    foward = true;
            }
            return playlist[playlistIndex];
        }

        /// <summary>
        /// Get a sound from <see cref="playlist"/>.<br/>
        /// Don't forget to use <see cref="Volume"/> for this <see cref="Playlist"/> master volume.
        /// </summary>
        /// <param name="mode">Playing mode. If <see langword="null"/>, default configured value in <see cref="playingMode"/> will be used.</param>
        /// <returns>Sound to play and its playlist <see cref="volume"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0066:Convert switch statement to expression", Justification = "Not supported by Unity.")]
        public Sound GetSound(PlayMode? mode = null)
        {
            if (playlist.Length == 0)
                throw new IndexOutOfRangeException($"Does not have any sound in {playlist}.");

            switch (mode ?? playingMode)
            {
                case PlayMode.Next:
                    return GetNextSound();
                case PlayMode.PingPong:
                    return GetPingPongSound();
                case PlayMode.Random:
                    return GetRandomSound();
                default:
                    throw new ImpossibleStateException();
            }
        }

        /// <summary>
        /// Reset the <see cref="playlistIndex"/> to 0.
        /// </summary>
        public void ResetIndex() => playlistIndex = 0;

        /// <summary>
        /// Play a sound from the <seealso cref="playlist"/> using a method described by <seealso cref="PlayMode"/>.<br/>
        /// </summary>
        /// <param name="audioSource"><see cref="AudioSource"/> to play the sound.</param>
        /// <param name="playMode">Mode to get the sound form the <seealso cref="playlist"/>.<br/>
        /// If <see langword="null"/>, default configured value in <see cref="playingMode"/> will be used.</param>
        /// <param name="volumeMultiplier">Multiplier of the volume, from 0 to 1.</param>
        public void Play(AudioSource audioSource, PlayMode? playMode = null, float volumeMultiplier = 1)
        {
            if (audioSource == null)
                throw new ArgumentNullException(nameof(audioSource));
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            GetSound(playMode).Play(audioSource, Volume * volumeMultiplier);
        }

        /// <summary>
        /// Play a sound from <see cref="playlist"/> given its name.
        /// </summary>
        /// <param name="audioSource"><see cref="AudioSource"/> to play the sound.</param>
        /// <param name="name">Name of the sound to look for in <see cref="playlist"/>.</param>
        /// <param name="volumeMultiplier">Multiplier of the volume, from 0 to 1.</param>
        /// <returns>Whenever the sound was found (and played) or not.</returns>
        public bool Play(AudioSource audioSource, string name, float volumeMultiplier = 1)
        {
            if (audioSource == null)
                throw new ArgumentNullException(nameof(audioSource));
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            Sound sound = Array.Find(playlist, e => e.Name == name);

            if (sound == null)
                return false;

            sound.Play(audioSource, Volume * volumeMultiplier);
            return true;
        }

        /// <summary>
        /// Play a sound from the <seealso cref="playlist"/> using a method described by <seealso cref="PlayMode"/> on the specified <paramref name="position"/>.
        /// </summary>
        /// <param name="position">Position to play the sound.</param>
        /// <param name="playMode">Mode to get the sound form the <seealso cref="playlist"/>.<br/>
        /// If <see langword="null"/>, default configured value in <see cref="playingMode"/> will be used.</param>
        /// <param name="volumeMultiplier">Multiplier of the volume, from 0 to 1.</param>
        public void PlayAtPoint(Vector3 position, PlayMode? playMode = null, float volumeMultiplier = 1)
        {
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            GetSound(playMode).PlayAtPoint(position, Volume * volumeMultiplier);
        }

        /// <summary>
        /// Play a sound from <see cref="playlist"/> given its name on the specified <paramref name="position"/>.
        /// </summary>
        /// <param name="position">Position to play the sound.</param>
        /// <param name="name">Name of the sound to look for in <see cref="playlist"/>.</param>
        /// <param name="volumeMultiplier">Multiplier of the volume, from 0 to 1.</param>
        /// <returns>Whenever the sound was found (and played) or not.</returns>
        public bool PlayAtPoint(Vector3 position, string name, float volumeMultiplier = 1)
        {
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            Sound sound = Array.Find(playlist, e => e.Name == name);

            if (sound == null)
                return false;

            sound.PlayAtPoint(position, Volume * volumeMultiplier);
            return true;
        }
    }
}
