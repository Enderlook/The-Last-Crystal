using UnityEngine;

[System.Serializable]
public class AudioSystem
{
#pragma warning disable CS0649
    [Header("Audio System")]
    [SerializeField, Tooltip("Audio Source component.")]
    private AudioSource audioGame;
    [SerializeField, Tooltip("Audio clips for slash.")]
    private AudioClip slashClips;
    [SerializeField, Tooltip("Audio clips for shoot")]
    private AudioClip shotClips;
    [SerializeField, Tooltip("Audio clips for footsteep.")]
    private AudioClip[] stepClips;
#pragma warning disable CS0649

    public void SlashSoundEffect() => audioGame.PlayOneShot(slashClips);

    public void ShootSoundEffect() => audioGame.PlayOneShot(shotClips);

    public void FootStepSoundEffect()
    {
        audioGame.PlayOneShot(RandomSound(stepClips));
    }

    private AudioClip RandomSound(AudioClip[] clips)
    {
        if (clips == null) return null;

        return clips[Random.Range(0, clips.Length)];
    }
}
