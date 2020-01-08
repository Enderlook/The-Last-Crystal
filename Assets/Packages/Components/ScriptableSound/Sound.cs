using System;

using UnityEngine;

namespace ScriptableSound
{
    public class Sound : ScriptableObject
    {
#pragma warning disable CS0649
        /// <summary>
        /// Amount of times to loop, if negative loop forever.
        /// </summary>
        [SerializeField, Tooltip("Loop amount. If 0 doesn't loop. If negative it loops forever.")]
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

        public virtual void Update() => throw new NotImplementedException("This class is 'abstract' and should not be instantiated by its own. Use derived classes instead which override this method.");
    }
}