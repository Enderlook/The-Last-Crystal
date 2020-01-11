using Range;

using UnityEngine;

using Utils;

namespace ScriptableSound.Modifiers
{
    [CreateAssetMenu(fileName = "VolumeRange", menuName = "Sound/Modifiers/Volume Range")]
    public class VolumeRangeModifier : SoundModifier
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Volume multiplier.")]
        private RangeFloat volumeMultiplier;
#pragma warning restore CS0649

        private float oldVolume;

        public override void ModifyAudioSource(AudioSource audioSource)
        {
            oldVolume = audioSource.volume;
            audioSource.volume *= volumeMultiplier.Value;
        }

        public override void BackToNormalAudioSource(AudioSource audioSource) => audioSource.volume = oldVolume;

        public override SoundModifier CreatePrototype()
        {
            VolumeRangeModifier prototype = CreateInstance<VolumeRangeModifier>();
            prototype.name = PrototypeHelper.GetPrototypeNameOf(prototype);
            prototype.volumeMultiplier = volumeMultiplier;
            return prototype;
        }
    }
}
