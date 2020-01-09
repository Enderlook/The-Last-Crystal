using AdditionalAttributes;
using ScriptableSound.Modifiers;
using System;

using UnityEngine;

namespace ScriptableSound
{
    [CreateAssetMenu(fileName = "SoundClip", menuName = "Sound/SoundClip")]
    public class SoundClip : Sound
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Audioclip to play.\nModifiers doesn't work if played in Inspector."), PlayAudioClip]
        private AudioClip audioClip;

        [SerializeField, Tooltip("Modifiers to AudioSource."), Expandable]
        private SoundModifier[] modifiers;
#pragma warning restore

        public override void Update()
        {
            if (ShouldChangeSound)
            {
                if (HasEnoughLoops)
                {
                    AudioSource audioSource = soundConfiguration.audioSource;
                    ReduceRemainingLoopsByOne();
                    Array.ForEach(modifiers, e => e.ModifyAudioSource(audioSource));
                    audioSource.clip = audioClip;
                    audioSource.Play();
                }
                else
                {
                    IsPlaying = false;
                    BackToNormalAudioSource();
                }
            }
        }

        private void BackToNormalAudioSource()
        {
            AudioSource audioSource = soundConfiguration.audioSource;
            for (int i = modifiers.Length - 1; i >= 0; i--)
                modifiers[i].BackToNormalAudioSource(audioSource);
            soundConfiguration.EndCallback();
        }

        public override void Stop()
        {
            BackToNormalAudioSource();
            base.Stop();
        }
    }
}