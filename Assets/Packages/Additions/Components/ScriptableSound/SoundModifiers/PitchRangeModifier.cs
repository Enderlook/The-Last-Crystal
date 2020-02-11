using Additions.Serializables.Ranges;
using Additions.Utils;

using System;

using UnityEngine;

namespace Additions.Components.ScriptableSound.Modifiers
{
    [Serializable, CreateAssetMenu(fileName = "PitchRange", menuName = "Sound/Modifiers/Pitch Range")]
    public class PitchRangeModifier : SoundModifier
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Pitch multiplier.")]
        private RangeFloat pitchMultiplier;
#pragma warning restore CS0649

        private float oldPitch;

        public override void ModifyAudioSource(AudioSource audioSource)
        {
            oldPitch = audioSource.pitch;
            audioSource.pitch *= pitchMultiplier.Value;
        }

        public override void BackToNormalAudioSource(AudioSource audioSource) => audioSource.pitch = oldPitch;

        public override SoundModifier CreatePrototype()
        {
            PitchRangeModifier prototype = CreateInstance<PitchRangeModifier>();
            prototype.name = PrototypeHelper.GetPrototypeNameOf(prototype);
            prototype.pitchMultiplier = pitchMultiplier;
            return prototype;
        }
    }
}