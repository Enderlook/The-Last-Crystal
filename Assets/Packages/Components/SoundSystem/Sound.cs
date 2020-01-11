using Additions.Serializables.Ranges;

using System;

using UnityEngine;

namespace Additions.Components.SoundSystem
{
    [Serializable]
    public class Sound : ISound
    {
        [SerializeField, Tooltip("Name of sound, used to be access by other scripts.")]
#pragma warning disable CS0649
        private string name;
        public string Name => name;

        [SerializeField, Tooltip("Sound clip.")]
        private AudioClip audioClip;
        public AudioClip AudioClip => audioClip;

        [SerializeField, Tooltip("Volume. Use range size 1 to avoid random volume. Use range size 0 to use 1.")]
        private RangeFloatSwitchable volume;
        public float Volume => volume.Value;

        [SerializeField, Tooltip("Pitch. Use range size 1 to avoid random volume. Use range size 0 to use 1.")]
        private RangeFloatSwitchable pitch;
        public float Pitch => pitch.Value;
#pragma warning restore CS0649

        public void PlayOneShoot(AudioSource audioSource, float volumeMultiplier = 1)
        {
            if (audioSource == null)
                throw new ArgumentNullException(nameof(audioSource));
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            audioSource.pitch = Pitch;
            audioSource.PlayOneShot(audioClip, Volume * volumeMultiplier);
        }

        public bool PlayOneShootIfNotPlaying(AudioSource audioSource, float volumeMultiplier = 1)
        {
            if (audioSource == null)
                throw new ArgumentNullException(nameof(audioSource));
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            if (audioSource.isPlaying)
                return false;

            PlayOneShoot(audioSource, volumeMultiplier);
            return true;
        }

        public void Play(AudioSource audioSource, float volumeMultiplier = 1)
        {
            if (audioSource == null)
                throw new ArgumentNullException(nameof(audioSource));
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));


            audioSource.pitch = Pitch;
            audioSource.volume = Volume * volumeMultiplier;
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        public void PlayAtPoint(Vector3 position, float volumeMultiplier = 1)
        {
            if (volumeMultiplier < 0 && volumeMultiplier > 1)
                throw new ArgumentException("Must be a number between 0 and 1", nameof(volumeMultiplier));

            AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier);
        }
    }
}