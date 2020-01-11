using System;

using UnityEngine;

namespace SoundSystem
{
    [Serializable]
    public class SoundWithAudioSource : ISound
    {
        [SerializeField, Tooltip("Sound to play.")]
#pragma warning disable CS0649
        private Sound sound;

        [SerializeField, Tooltip("AudioSource used to play.")]
        private AudioSource audioSource;
#pragma warning restore CS0649

        /// <summary>
        /// Play the <see cref="sound"/>.
        /// </summary>
        /// <param name="volumeMultiplier">Volume of the sound, from 0 to 1.</param>
        public void Play(float volumeMultiplier = 1)
        {
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            sound.Play(audioSource, volumeMultiplier);
        }

        /// <summary>
        /// Play the sound on the specified <paramref name="audioSource"/>.
        /// </summary>
        /// <param name="volumeMultiplier">Volume of the sound, from 0 to 1.</param>
        public void PlayOneShoot(float volumeMultiplier = 1)
        {
            if (audioSource == null)
                throw new ArgumentNullException(nameof(audioSource));
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            sound.PlayOneShoot(audioSource, volumeMultiplier);
        }

        /// <summary>
        /// Play the sound on the specified <paramref name="audioSource"/> if it is not already playing a sound..
        /// </summary>
        /// <param name="volumeMultiplier">Volume of the sound, from 0 to 1.</param>
        /// <returns>Whenever it could play or there was already a sound being played.</returns>
        public bool PlayOneShootIfNotPlaying(float volumeMultiplier = 1)
        {
            if (audioSource == null)
                throw new ArgumentNullException(nameof(audioSource));
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            return sound.PlayOneShootIfNotPlaying(audioSource, volumeMultiplier);
        }

        /// <summary>
        /// Play the sound on the specified <paramref name="position"/>.
        /// </summary>
        /// <param name="position">Position to play the sound.</param>
        /// <param name="volumeMultiplier">Volume of the sound, from 0 to 1.</param>
        public void PlayAtPoint(Vector3 position, float volumeMultiplier = 1)
        {
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            AudioSource.PlayClipAtPoint(sound.AudioClip, position, volumeMultiplier);
        }

        public void PlayOneShoot(AudioSource audioSource, float volumeMultiplier = 1) => ((ISound)sound).PlayOneShoot(audioSource, volumeMultiplier);
        public bool PlayOneShootIfNotPlaying(AudioSource audioSource, float volumeMultiplier = 1) => ((ISound)sound).PlayOneShootIfNotPlaying(audioSource, volumeMultiplier);
        public void Play(AudioSource audioSource, float volumeMultiplier = 1) => ((ISound)sound).Play(audioSource, volumeMultiplier);
    }
}