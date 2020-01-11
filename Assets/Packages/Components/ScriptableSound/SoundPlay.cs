using AdditionalAttributes;

using System;

using UnityEngine;

namespace ScriptableSound
{
    [Serializable]
    public class SoundPlay
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("AudioSource where sound is played.")]
        private AudioSource audioSource;

        [SerializeField, Tooltip("Sound to play."), Expandable]
        private Sound sound;
#pragma warning restore CS0649

        public void Init() => sound = sound.CreatePrototype();

        public void UpdateBehaviour(float deltaTime) => sound.UpdateBehaviour(deltaTime);

        public void Play(Action endCallback = null)
        {
            sound.SetConfiguration(new SoundConfiguration(audioSource, endCallback));
            sound.Play();
        }
    }
}