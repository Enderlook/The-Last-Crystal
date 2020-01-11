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

        [SerializeField, Min(-1), Tooltip("Amount of times it will play when called Play method. If 0 doesn't play. If negative it loops forever.")]
        private int playsAmount = 1;

        private int remainingPlays;

        [SerializeField, Tooltip("Modifiers to AudioSource."), Expandable]
        private SoundModifier[] modifiers;
#pragma warning restore

        private bool HasEnoughPlays()
        {
            bool can = remainingPlays > 0 || playsAmount == -1;
            remainingPlays--;
            return can;
        }

        public override void Update()
        {
            if (ShouldChangeSound)
            {
                if (HasEnoughPlays())
                {
                    AudioSource audioSource = soundConfiguration.audioSource;
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

        public override void Play()
        {
            remainingPlays = playsAmount;
            base.Play();
        }
    }
}