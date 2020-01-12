using System;

using UnityEngine;

namespace Additions.Components.ScriptableSound
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