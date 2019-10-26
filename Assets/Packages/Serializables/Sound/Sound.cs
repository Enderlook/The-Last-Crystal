using System;
using Range;
using UnityEngine;

[Serializable]
public class Sound
{
    [Tooltip("Sound clip.")]
    public AudioClip audioClip;

    [Tooltip("Volume. Use range size 1 to avoid random volume. Use range size 0 to use 1.")]
    public RangeFloatSwitchable volume;

    [Tooltip("Pitch. Use range size 1 to avoid random volume. Use range size 0 to use 1.")]
    public RangeFloatSwitchable pitch;

    /// <summary>
    /// Calculates a random volume between the given by the two first elements of <see cref="volume"/>.
    /// </summary>
    /// <returns>Random volume. If <c><see cref="volume"/>.lenght <= 1</c> it <see langword="return"/> the <c><see cref="volume"/>[1]</c>.</returns>
    /// <seealso cref="GetRandom(float[])"/>
    public float Volume => volume.Value;

    /// <summary>
    /// Calculates a random pitch between the given by the two first elements of <see cref="pitch"/>.
    /// </summary>
    /// <returns>Random volume. If <c><see cref="pitch"/>.lenght <= 1</c> it <see langword="return"/> the <c><see cref="pitch"/>[1]</c>.</returns>
    /// <seealso cref="GetRandom(float[] array)"/>
    public float Pitch => pitch.Value;

    /// <summary>
    /// Play the sound on the specified <paramref name="audioSource"/>.
    /// </summary>
    /// <param name="audioSource"><see cref="AudioSource"/> where the sound will be played.</param>
    /// <param name="isSoundActive">Whenever sound is active or not. On <see langword="false"/> no sound will be played.</param>
    /// <param name="volumeMultiplier">Volume of the sound, from 0 to 1.</param>
    public void PlayOneShoot(AudioSource audioSource, bool isSoundActive, float volumeMultiplier = 1)
    {
        // audioSource != null shouldn't be used but it's to prevents a bug
        if (isSoundActive && audioSource != null)
        {
            audioSource.pitch = Pitch;
            audioSource.PlayOneShot(audioClip, Volume * volumeMultiplier);
        }
    }

    /// <summary>
    /// Play the sound on the specified <paramref name="audioSource"/>.
    /// </summary>
    /// <param name="audioSource"><see cref="AudioSource"/> where the sound will be played.</param>
    /// <param name="isSoundActive">Whenever sound is active or not. On <see langword="false"/> no sound will be played.</param>
    /// <param name="volumeMultiplier">Volume of the sound, from 0 to 1.</param>
    public void Play(AudioSource audioSource, bool isSoundActive, float volumeMultiplier = 1)
    {
        // audioSource != null shouldn't be used but it's to prevents a bug
        if (isSoundActive && audioSource != null)
        {
            audioSource.pitch = Pitch;
            audioSource.volume = Volume * volumeMultiplier;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    /// <summary>
    /// Play the sound on the specified <paramref name="position"/>.
    /// </summary>
    /// <param name="position">Position to play the sound.</param>
    /// <param name="isSoundActive">Whenever sound is active or not. On <see langword="false"/> no sound will be played.</param>
    /// <param name="volumeMultiplier">Volume of the sound, from 0 to 1.</param>
    public void PlayAtPoint(Vector3 position, bool isSoundActive, float volumeMultiplier = 1)
    {
        if (isSoundActive)
            AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier);
    }
}