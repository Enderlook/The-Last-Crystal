using System;

using UnityEngine;

namespace ScriptableSound
{
    public class SoundConfiguration
    {
        public readonly AudioSource audioSource;
        public readonly Action EndCallback;

        public SoundConfiguration(AudioSource audioSource, Action EndCallback = null)
        {
            this.audioSource = audioSource;
            this.EndCallback = EndCallback ?? DoNothing;
        }

        private static void DoNothing() { }
    }
}