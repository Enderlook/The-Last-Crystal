﻿using AdditionalAttributes;

using System;

using UnityEngine;

namespace ScriptableSound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : MonoBehaviour
    {
        private AudioSource audioSource;

        [SerializeField, Tooltip("List of sounds to play."), Expandable]
        private Sound[] sounds;

        [SerializeField, Tooltip("If start playing on awake.")]
        private bool playOnAwake;

        [SerializeField, Tooltip("Which playlist play on awake."), ShowIf(nameof(playOnAwake), indented = true)]
        private int onAwakeIndex;

        private int index;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (playOnAwake)
                Play(onAwakeIndex);
        }

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