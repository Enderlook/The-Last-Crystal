﻿using UnityEngine;

namespace ScriptableSound.Modifiers
{
    [CreateAssetMenu(fileName = "Volume", menuName = "Sound/Modifiers/Volume")]
    public class VolumeModifier : SoundModifier
    {
        [SerializeField, Tooltip("Volume multiplier.")]
        private float volumeMultiplier = 1;

        private float oldVolume;

        public override void ModifyAudioSource(AudioSource audioSource)
        {
            oldVolume = audioSource.volume;
            audioSource.volume *= volumeMultiplier;
        }

        public override void BackToNormalAudioSource(AudioSource audioSource) => audioSource.volume = oldVolume;
    }
}