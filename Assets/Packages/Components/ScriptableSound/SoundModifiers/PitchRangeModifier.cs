﻿using Range;

using UnityEngine;

namespace ScriptableSound.Modifiers
{
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
    }
}