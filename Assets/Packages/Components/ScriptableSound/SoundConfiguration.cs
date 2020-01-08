using System;

using UnityEngine;

namespace ScriptableSound
{
    public class SoundConfiguration
    {
        public readonly AudioSource audioSource;
        public readonly Func<float> GetVolume;
        public readonly Func<float> GetPitch;
        public readonly Action EndCallback;

        public SoundConfiguration(AudioSource audioSource, Func<float> GetVolume = null, Func<float> GetPitch = null, Action EndCallback = null)
        {
            this.audioSource = audioSource;
            this.GetVolume = GetVolume ?? GetOne;
            this.GetPitch = GetPitch ?? GetOne;
            this.EndCallback = EndCallback ?? DoNothing;
        }

        private static float GetOne() => 1;
        private static void DoNothing() { }
    }
}