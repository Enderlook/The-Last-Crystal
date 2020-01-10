using System;
using UnityEngine;

namespace SoundSystem
{
    [Serializable]
    public class PlaylistWithAudioSource : IPlaylist
    {
        [SerializeField, Tooltip("Sounds to play.")]
#pragma warning disable CS0649
        private Playlist playlist;

        [SerializeField, Tooltip("AudioSource used to play.")]
        private AudioSource audioSource;
#pragma warning restore CS0649

        public void Play(AudioSource audioSource, PlayMode playMode, float volumeMultiplier = 1) => ((IPlaylist)playlist).Play(audioSource, playMode, volumeMultiplier);
        public bool Play(AudioSource audioSource, string name, float volumeMultiplier = 1) => ((IPlaylist)playlist).Play(audioSource, name, volumeMultiplier);
        public void PlayAtPoint(Vector3 position, PlayMode playMode, float volumeMultiplier = 1) => ((IPlaylist)playlist).PlayAtPoint(position, playMode, volumeMultiplier);
        public bool PlayAtPoint(Vector3 position, string name, float volumeMultiplier = 1) => ((IPlaylist)playlist).PlayAtPoint(position, name, volumeMultiplier);
        public void PlayOneShoot(AudioSource audioSource, PlayMode playMode, float volumeMultiplier = 1) => ((IPlaylist)playlist).PlayOneShoot(audioSource, playMode, volumeMultiplier);
        public bool PlayOneShoot(AudioSource audioSource, string name, float volumeMultiplier = 1) => ((IPlaylist)playlist).PlayOneShoot(audioSource, name, volumeMultiplier);
        public bool PlayOneShootIfNotPlaying(AudioSource audioSource, PlayMode playMode, float volumeMultiplier = 1) => ((IPlaylist)playlist).PlayOneShootIfNotPlaying(audioSource, playMode, volumeMultiplier);
        public bool PlayOneShootIfNotPlaying(AudioSource audioSource, string name, float volumeMultiplier = 1) => ((IPlaylist)playlist).PlayOneShootIfNotPlaying(audioSource, name, volumeMultiplier);
    }
}