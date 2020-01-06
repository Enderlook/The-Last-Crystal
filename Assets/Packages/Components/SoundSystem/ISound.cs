using UnityEngine;

namespace SoundSystem
{
    public interface ISound
    {
        /// <summary>
        /// Play the sound on the specified <paramref name="audioSource"/>.
        /// </summary>
        /// <param name="audioSource"><see cref="AudioSource"/> where the sound will be played.</param>
        /// <param name="volumeMultiplier">Volume of the sound, from 0 to 1.</param>
        void PlayOneShoot(AudioSource audioSource, float volumeMultiplier = 1);

        /// <summary>
        /// Play the sound on the specified <paramref name="audioSource"/> if it is not already playing a sound.
        /// </summary>
        /// <param name="audioSource"><see cref="AudioSource"/> where the sound will be played.</param>
        /// <param name="volumeMultiplier">Volume of the sound, from 0 to 1.</param>
        /// <returns>Whenever it could play or there was already a sound being played.</returns>
        bool PlayOneShootIfNotPlaying(AudioSource audioSource, float volumeMultiplier = 1);

        /// <summary>
        /// Play the sound on the specified <paramref name="audioSource"/>.
        /// </summary>
        /// <param name="audioSource"><see cref="AudioSource"/> where the sound will be played.</param>
        /// <param name="volumeMultiplier">Volume of the sound, from 0 to 1.</param>
        void Play(AudioSource audioSource, float volumeMultiplier = 1);

        /// <summary>
        /// Play the sound on the specified <paramref name="position"/>.
        /// </summary>
        /// <param name="position">Position to play the sound.</param>
        /// <param name="volumeMultiplier">Volume of the sound, from 0 to 1.</param>
        void PlayAtPoint(Vector3 position, float volumeMultiplier = 1);
    }
}