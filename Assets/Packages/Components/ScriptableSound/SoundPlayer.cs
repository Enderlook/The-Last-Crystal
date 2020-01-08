using AdditionalAttributes;
using System;
using UnityEngine;

namespace ScriptableSound
{
    public class SoundPlayer : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField, Tooltip("AudioSource used to play sound.")]
        private AudioSource audioSource;

        [SerializeField, Tooltip("List of sounds to play."), Expandable]
        private Sound[] sounds;

        private int index;

        public void Play(int index, Action endCallback = null)
        {
            Sound sound = sounds[index];
            sound.SetConfiguration(new SoundConfiguration(audioSource, endCallback));
            sound.Play();
            this.index = index;
        }

        private void Update() => sounds[index].Update();
    }
}