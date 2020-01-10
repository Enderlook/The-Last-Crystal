using AdditionalAttributes;

using System;

using UnityEngine;

namespace ScriptableSound
{
    [Serializable]
    public class SoundPlay
    {
        [SerializeField, Tooltip("AudioSource where sound is played.")]
        private AudioSource audioSource;

        [SerializeField, Tooltip("Sound to play."), Expandable]
        private Sound sound;

        public void Update() => sound.Update();

        public void Play(Action endCallback = null)
        {
            sound.SetConfiguration(new SoundConfiguration(audioSource, endCallback));
            sound.Play();
        }
    }
}