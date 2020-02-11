using Additions.Utils;

using System;

using UnityEngine;

namespace Additions.Components.ScriptableSound.Modifiers
{
    [Serializable, CreateAssetMenu(fileName = "Pitch", menuName = "Sound/Modifiers/Pitch")]
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

        public override SoundModifier CreatePrototype()
        {
            PitchModifier prototype = CreateInstance<PitchModifier>();
            prototype.name = PrototypeHelper.GetPrototypeNameOf(prototype);
            prototype.pitchMultiplier = pitchMultiplier;
            return prototype;
        }
    }
}
