using Range;

using System;

using UnityEngine;

namespace SoundSystem
{
    [Serializable]
    public class Sound
    {
        [SerializeField, Tooltip("Sound clip.")]
#pragma warning disable CS0649
        private AudioClip audioClip;
        public AudioClip AudioClip => audioClip;

        [SerializeField, Tooltip("Volume. Use range size 1 to avoid random volume. Use range size 0 to use 1.")]
        private RangeFloatSwitchable volume;
        public float Volume => volume.Value;

        [SerializeField, Tooltip("Pitch. Use range size 1 to avoid random volume. Use range size 0 to use 1.")]
        private RangeFloatSwitchable pitch;
        public float Pitch => pitch.Value;
#pragma warning restore CS0649

        /// <summary>
        /// Play the sound on the specified <paramref name="audioSource"/>.
        /// </summary>
        /// <param name="audioSource"><see cref="AudioSource"/> where the sound will be played.</param>
        /// <param name="volumeMultiplier">Volume of the sound, from 0 to 1.</param>
        public void PlayOneShoot(AudioSource audioSource, float volumeMultiplier = 1)
        {
            if (audioSource == null)
                throw new ArgumentNullException(nameof(audioSource));

            audioSource.pitch = Pitch;
                audioSource.PlayOneShot(audioClip, Volume * volumeMultiplier);
        }

        /// <summary>
        /// Play the sound on the specified <paramref name="audioSource"/>.
        /// </summary>
        /// <param name="audioSource"><see cref="AudioSource"/> where the sound will be played.</param>
        /// <param name="volumeMultiplier">Volume of the sound, from 0 to 1.</param>
        public void Play(AudioSource audioSource, float volumeMultiplier = 1)
        {
            if (audioSource == null)
                throw new ArgumentNullException(nameof(audioSource));

            audioSource.pitch = Pitch;
            audioSource.volume = Volume * volumeMultiplier;
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        /// <summary>
        /// Play the sound on the specified <paramref name="position"/>.
        /// </summary>
        /// <param name="position">Position to play the sound.</param>
        /// <param name="volumeMultiplier">Volume of the sound, from 0 to 1.</param>
        public void PlayAtPoint(Vector3 position, float volumeMultiplier = 1) => AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier);
    }
}