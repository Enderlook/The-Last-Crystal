using UnityEngine;

namespace ScriptableSound
{
    [CreateAssetMenu(fileName = "SoundClip", menuName = "SoundClip")]
    public class SoundClip : Sound
    {
        [Header("Setup")]
        [SerializeField, Tooltip("Audioclip to play.")]
        protected AudioClip audioClip;

        public override void Update()
        {
            if (ShouldChangeSound)
            {
                if (HasEnoughLoops)
                {
                    ReduceRemainingLoopsByOne();
                    AudioSource audioSource = soundConfiguration.audioSource;
                    audioSource.pitch = GetPitch();
                    audioSource.PlayOneShot(audioClip, GetVolume());
                }
                else
                {
                    IsPlaying = false;
                    soundConfiguration.EndCallback();
                }
            }
        }
    }
}