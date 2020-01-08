
using UnityEngine;

namespace ScriptableSound.Modifiers
{
    public class PitchModifier : SoundModifier
    {
        [SerializeField, Tooltip("Pitch multiplier.")]
        private float pitchMultiplier = 1;

        private float oldPitch;

        public override void ModifyAudioSource(AudioSource audioSource)
        {
            oldPitch = audioSource.pitch;
            audioSource.pitch *= pitchMultiplier;
        }

        public override void BackToNormalAudioSource(AudioSource audioSource) => audioSource.pitch = oldPitch;
    }
}
