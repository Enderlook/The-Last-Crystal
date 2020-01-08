using Range;

using System;

using UnityEngine;

namespace ScriptableSound
{
    public class Sound : ScriptableObject
    {
#pragma warning disable CS0649
        [Header("Configuration")]
        [SerializeField, Tooltip("Volume configuration.")]
        private RangeFloatSwitchable volume;

        [SerializeField, Tooltip("Pitch configuration.")]
        private RangeFloatSwitchable pitch;

        /// <summary>
        /// Amount of times to loop, if negative loop forever.
        /// </summary>
        [SerializeField, Tooltip("Loop amount. If negative it loops forever.")]
        private int loops;

        /// <summary>
        /// Remaining loops to end, ignore this if <c><see cref="loops"/> == -1</c>.
        /// </summary>
        private int remainingLoops;

        /// <summary>
        /// Check if there are enough loops to keep playing.
        /// </summary>
        protected bool HasEnoughLoops => remainingLoops > 0 || loops == -1;

        /// <summary>
        /// Reduce <see cref="remainingLoops"/> by one.
        /// </summary>
        protected void ReduceRemainingLoopsByOne() => remainingLoops--;

        /// <summary>
        /// Whenever this class is playing music.
        /// </summary>
        public bool IsPlaying { get; protected set; }

        /// <summary>
        /// Configuration.
        /// </summary>
        protected SoundConfiguration soundConfiguration;
#pragma warning restore

        /// <summary>
        /// Configure the sound.
        /// </summary>
        /// <param name="soundConfiguration">Configuration.</param>
        public void SetConfiguration(SoundConfiguration soundConfiguration) => this.soundConfiguration = soundConfiguration;

        /// <summary>
        /// Play sound(s).
        /// </summary>
        public void Play()
        {
            if (soundConfiguration == null)
                throw new InvalidOperationException($"The method {nameof(SetConfiguration)} must be called first.");
            remainingLoops = loops + 1;
            IsPlaying = true;
        }

        /// <summary>
        /// Check if it's playing and the current clip has reached end.
        /// </summary>
        /// <remarks>
        /// Use <see cref="AudioSource.time"/> instead of <see cref="AudioSource.isPlaying"/> because the second one produce wrong results if <see cref="AudioSource"/> is paused.
        /// </remarks>
        protected bool ShouldChangeSound => IsPlaying && soundConfiguration.audioSource.time == 0;

        /// <summary>
        /// Get the volume to play taking into account the <see cref="soundConfiguration"/>.
        /// </summary>
        /// <returns>Volume to play.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the product between <see cref="soundConfiguration"/> <see cref="SoundConfiguration.GetVolume"/> and <see cref="volume"/> is not in range [0; 1].</exception>
        public float GetVolume()
        {
            float v = soundConfiguration.GetVolume() * volume.Value;
            if (v > 1 || v < 0)
                throw new ArgumentOutOfRangeException($"The product between {nameof(soundConfiguration.GetVolume)} and {nameof(volume.Value)} must be a number from 0 to 1. Was {v}.");
            return v;
        }

        /// <summary>
        /// Get the pitch to play taking into account the <see cref="soundConfiguration"/>.
        /// </summary>
        /// <returns>Pitch to play.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the product between <see cref="soundConfiguration"/> <see cref="SoundConfiguration.GetPitch"/> and <see cref="pitch"/> is not lesser than 0.</exception>
        public float GetPitch()
        {
            float v = soundConfiguration.GetPitch() * pitch.Value;
            if (v < 0)
                throw new ArgumentOutOfRangeException($"The product between {nameof(soundConfiguration.GetPitch)} and {nameof(pitch.Value)} must be greater than 0. Was {v}.");
            return v;
        }

        public virtual void Update() => throw new NotImplementedException("This class is 'abstract' and should not be instantiated by its own. Use derived classes instead which override this method.");
    }
}