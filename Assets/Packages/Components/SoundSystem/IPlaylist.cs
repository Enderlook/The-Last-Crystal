using UnityEngine;

namespace Additions.Components.SoundSystem
{
    public interface IPlaylist
    {
        /// <summary>
        /// Play a sound from the <seealso cref="playlist"/> using a method described by <seealso cref="PlayMode"/>.<br/>
        /// </summary>
        /// <param name="audioSource"><see cref="AudioSource"/> to play the sound.</param>
        /// <param name="playMode">Mode to get the sound form the <seealso cref="playlist"/>.
        /// <param name="volumeMultiplier">Multiplier of the volume, from 0 to 1.</param>
        void Play(AudioSource audioSource, PlayMode playMode, float volumeMultiplier = 1);

        /// <summary>
        /// Play a sound from <see cref="playlist"/> given its name.
        /// </summary>
        /// <param name="audioSource"><see cref="AudioSource"/> to play the sound.</param>
        /// <param name="name">Name of the sound to look for in <see cref="playlist"/>.</param>
        /// <param name="volumeMultiplier">Multiplier of the volume, from 0 to 1.</param>
        /// <returns>Whenever the sound was found (and played) or not.</returns>
        bool Play(AudioSource audioSource, string name, float volumeMultiplier = 1);

        /// <summary>
        /// Play a sound from the <seealso cref="playlist"/> using a method described by <seealso cref="PlayMode"/> on the specified <paramref name="position"/>.
        /// </summary>
        /// <param name="position">Position to play the sound.</param>
        /// <param name="playMode">Mode to get the sound form the <seealso cref="playlist"/>
        /// <param name="volumeMultiplier">Multiplier of the volume, from 0 to 1.</param>
        void PlayAtPoint(Vector3 position, PlayMode playMode, float volumeMultiplier = 1);

        /// <summary>
        /// Play a sound from <see cref="playlist"/> given its name on the specified <paramref name="position"/>.
        /// </summary>
        /// <param name="position">Position to play the sound.</param>
        /// <param name="name">Name of the sound to look for in <see cref="playlist"/>.</param>
        /// <param name="volumeMultiplier">Multiplier of the volume, from 0 to 1.</param>
        /// <returns>Whenever the sound was found (and played) or not.</returns>
        bool PlayAtPoint(Vector3 position, string name, float volumeMultiplier = 1);

        /// <summary>
        /// Play the sound on the specified <paramref name="audioSource"/>.
        /// </summary>
        /// <param name="audioSource"><see cref="AudioSource"/> where the sound will be played.</param>
        /// <param name="playMode">Mode to get the sound form the <seealso cref="playlist"/>.
        /// <param name="volumeMultiplier">Volume of the sound, from 0 to 1.</param>
        void PlayOneShoot(AudioSource audioSource, PlayMode playMode, float volumeMultiplier = 1);

        /// <summary>
        /// Play the sound on the specified <paramref name="audioSource"/>.
        /// </summary>
        /// <param name="audioSource"><see cref="AudioSource"/> where the sound will be played.</param>
        /// <param name="name">Name of the sound to look for in <see cref="playlist"/>.</param>
        /// <param name="volumeMultiplier">Volume of the sound, from 0 to 1.</param>
        bool PlayOneShoot(AudioSource audioSource, string name, float volumeMultiplier = 1);

        /// <summary>
        /// Play the sound on the specified <paramref name="audioSource"/> if it is not already playing a sound.
        /// </summary>
        /// <param name="audioSource"><see cref="AudioSource"/> where the sound will be played.</param>
        /// <param name="playMode">Mode to get the sound form the <seealso cref="playlist"/>.
        /// <param name="volumeMultiplier">Volume of the sound, from 0 to 1.</param>
        /// <returns>Whenever it could play or there was already a sound being played.</returns>
        bool PlayOneShootIfNotPlaying(AudioSource audioSource, PlayMode playMode, float volumeMultiplier = 1);

        /// <summary>
        /// Play the sound on the specified <paramref name="audioSource"/> if it is not already playing a sound.
        /// </summary>
        /// <param name="audioSource"><see cref="AudioSource"/> where the sound will be played.</param>
        /// <param name="name">Name of the sound to look for in <see cref="playlist"/>.</param>
        /// <param name="volumeMultiplier">Volume of the sound, from 0 to 1.</param>
        /// <returns>Whenever it could play or there was already a sound being played or the sound could not be found.</returns>
        bool PlayOneShootIfNotPlaying(AudioSource audioSource, string name, float volumeMultiplier = 1);
    }
}