using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Sound
{
    [Tooltip("Sound clip.")]
    public AudioClip audioClip;

    [Tooltip("Volume. Use range size 1 to avoid random volume. Use range size 0 to use 1.")]
    public float[] volume = new float[1] { 1 };

    [Tooltip("Pitch. Use range size 1 to avoid random volume. Use range size 0 to use 1.")]
    public float[] pitch = new float[1] { 1 };

    /// <summary>
    /// Calculates a random volume between the given by the two first elements of <see cref="volume"/>.
    /// </summary>
    /// <returns>Random volume. If <c><see cref="volume"/>.lenght <= 1</c> it <see langword="return"/> the <c><see cref="volume"/>[1]</c>.</returns>
    /// <seealso cref="GetRandom(float[])"/>
    public float Volume => GetRandom(volume);

    /// <summary>
    /// Calculates a random pitch between the given by the two first elements of <see cref="pitch"/>.
    /// </summary>
    /// <returns>Random volume. If <c><see cref="pitch"/>.lenght <= 1</c> it <see langword="return"/> the <c><see cref="pitch"/>[1]</c>.</returns>
    /// <seealso cref="GetRandom(float[] array)"/>
    public float Pitch => GetRandom(pitch);

    /// <summary>
    /// Calculates a random value between the given by the two first elements of <paramref name="array"/>.
    /// </summary>
    /// <param name="array"></param>
    /// <returns>Random volume. If <c><paramref name="array"/>.lenght <= 1</c> it <see langword="return"/> the <c><paramref name="array"/>[1]</c>.</returns>
    private float GetRandom(float[] array)
    {
        if (array.Length > 1)
            return Random.Range(array[0], array[1]);
        else if (array.Length == 0)
            return 1;
        else
            return array[0];
    }

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

