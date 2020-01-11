using Additions.Exceptions;

using System;

using UnityEngine;

using Random = UnityEngine.Random;

namespace Additions.Components.SoundSystem
{
    [CreateAssetMenu(fileName = "Playlist", menuName = "Playlist")]
    public class Playlist : ScriptableObject, IPlaylist, ISound
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
        /// <param name="mode">Playing mode.</param>
        /// <returns>Sound to play.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0066:Convert switch statement to expression", Justification = "Not supported by Unity.")]
        public Sound GetSound(PlayMode mode)
        {
            if (playlist.Length == 0)
                throw new IndexOutOfRangeException($"Does not have any sound in {playlist}.");

            switch (mode)
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
        /// Get a sound from <see cref="playlist"/> by name.<br/>
        /// Don't forget to use <see cref="Volume"/> for this <see cref="Playlist"/> master volume.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <returns>Sound to play.</returns>
        public Sound GetSound(string name) => Array.Find(playlist, e => e.Name == name);

        /// <summary>
        /// Get a sound from <see cref="playlist"/> by name.<br/>
        /// Don't forget to use <see cref="Volume"/> for this <see cref="Playlist"/> master volume.
        /// </summary>
        /// <param name="name">Name of the sound.</param>
        /// <param name="sound">Sound to play.</param>
        /// <returns>Whenever the sound by the given <paramref name="name"/> was found or not.</returns>
        public bool GetSound(string name, out Sound sound)
        {
            sound = GetSound(name);
            return sound != null;
        }

        /// <summary>
        /// Reset the <see cref="playlistIndex"/> to 0.
        /// </summary>
        public void ResetIndex() => playlistIndex = 0;

        public void Play(AudioSource audioSource, PlayMode playMode, float volumeMultiplier = 1)
        {
            if (audioSource == null)
                throw new ArgumentNullException(nameof(audioSource));
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            GetSound(playMode).Play(audioSource, Volume * volumeMultiplier);
        }

        public void PlayOneShoot(AudioSource audioSource, PlayMode playMode, float volumeMultiplier = 1)
        {
            if (audioSource == null)
                throw new ArgumentNullException(nameof(audioSource));
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            GetSound(playMode).PlayOneShoot(audioSource, Volume * volumeMultiplier);
        }

        public bool PlayOneShootIfNotPlaying(AudioSource audioSource, PlayMode playMode, float volumeMultiplier = 1)
        {
            if (audioSource == null)
                throw new ArgumentNullException(nameof(audioSource));
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            if (audioSource.isPlaying)
                return false;

            GetSound(playMode).PlayOneShootIfNotPlaying(audioSource, Volume * volumeMultiplier);
            return true;
        }

        public bool Play(AudioSource audioSource, string name, float volumeMultiplier = 1)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (name.Length == 0)
                throw new ArgumentException("Can't be empty", nameof(name));
            if (audioSource == null)
                throw new ArgumentNullException(nameof(audioSource));
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            if (GetSound(name, out Sound sound))
            {
                sound.Play(audioSource, Volume * volumeMultiplier);
                return true;
            }
            return false;
        }

        public void PlayAtPoint(Vector3 position, PlayMode playMode, float volumeMultiplier = 1)
        {
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            GetSound(playMode).PlayAtPoint(position, Volume * volumeMultiplier);
        }

        public bool PlayAtPoint(Vector3 position, string name, float volumeMultiplier = 1)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (name.Length == 0)
                throw new ArgumentException("Can't be empty", nameof(name));
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            if (GetSound(name, out Sound sound))
            {
                sound.PlayAtPoint(position, Volume * volumeMultiplier);
                return true;
            }
            return false;
        }

        public void PlayOneShoot(AudioSource audioSource, float volumeMultiplier = 1)
        {
            if (audioSource == null)
                throw new ArgumentNullException(nameof(audioSource));
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            PlayOneShoot(audioSource, PlayingMode, volumeMultiplier);
        }

        public bool PlayOneShoot(AudioSource audioSource, string name, float volumeMultiplier = 1)
        {
            if (audioSource == null)
                throw new ArgumentNullException(nameof(audioSource));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (name.Length == 0)
                throw new ArgumentException("Can't be empty", nameof(name));
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            if (GetSound(name, out Sound sound))
            {
                sound.PlayOneShoot(audioSource, Volume * volumeMultiplier);
                return true;
            }
            return false;
        }

        public bool PlayOneShootIfNotPlaying(AudioSource audioSource, float volumeMultiplier = 1)
        {
            if (audioSource == null)
                throw new ArgumentNullException(nameof(audioSource));
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            return PlayOneShootIfNotPlaying(audioSource, PlayingMode, volumeMultiplier);
        }

        public bool PlayOneShootIfNotPlaying(AudioSource audioSource, string name, float volumeMultiplier = 1)
        {
            if (audioSource == null)
                throw new ArgumentNullException(nameof(audioSource));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (name.Length == 0)
                throw new ArgumentException("Can't be empty", nameof(name));
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            if (!audioSource.isPlaying && GetSound(name, out Sound sound))
            {
                sound.PlayOneShootIfNotPlaying(audioSource, Volume * volumeMultiplier);
                return true;
            }
            return false;
        }

        public void Play(AudioSource audioSource, float volumeMultiplier = 1)
        {
            if (audioSource == null)
                throw new ArgumentNullException(nameof(audioSource));
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            Play(audioSource, PlayingMode, volumeMultiplier);
        }

        public void PlayAtPoint(Vector3 position, float volumeMultiplier = 1)
        {
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            PlayAtPoint(position, PlayingMode, volumeMultiplier);
        }
    }
}