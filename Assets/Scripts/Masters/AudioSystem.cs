using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class AudioSystem
{
#pragma warning disable CS0649, CA2235
    [Header("Audio System")]
    [SerializeField, Tooltip("Audio Source component.")]
    private AudioSource audioGame;
    [SerializeField, Tooltip("Audio clips for slash.")]
    private AudioClip slashClips;
    [SerializeField, Tooltip("Audio clips for shoot")]
    private AudioClip shotClips;
    [SerializeField, Tooltip("Audio clips for footsteep.")]
    private AudioClip[] stepClips;
#pragma warning disable CS0649, CA2235

    public void SlashSoundEffect() => audioGame.PlayOneShot(slashClips);

    public void ShootSoundEffect() => audioGame.PlayOneShot(shotClips);

    public void FootStepSoundEffect() => audioGame.PlayOneShot(RandomSound(stepClips));

    private static AudioClip RandomSound(AudioClip[] clips) => clips == null ? null : clips[Random.Range(0, clips.Length)];
}