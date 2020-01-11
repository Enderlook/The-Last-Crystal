﻿using AdditionalAttributes;

using System;

using UnityEngine;

namespace ScriptableSound
{
    [AbstractScriptableObject]
    public class Sound : ScriptableObject
    {
        /// <summary>
        /// Whenever this class is playing music.
        /// </summary>
        public bool IsPlaying { get; protected set; }

        /// <summary>
        /// Configuration.
        /// </summary>
        protected SoundConfiguration soundConfiguration;

        /// <summary>
        /// Configure the sound.
        /// </summary>
        /// <param name="soundConfiguration">Configuration.</param>
        public void SetConfiguration(SoundConfiguration soundConfiguration)
        {
            if (soundConfiguration == null) throw new ArgumentNullException(nameof(soundConfiguration));

            this.soundConfiguration = soundConfiguration;
        }

        /// <summary>
        /// Play sound(s).
        /// </summary>
        public virtual void Play()
        {
            if (soundConfiguration == null)
                throw new InvalidOperationException($"The method {nameof(SetConfiguration)} must be called first.");
            IsPlaying = true;
        }

        /// <summary>
        /// Stop sound.
        /// </summary>
        public virtual void Stop()
        {
            if (IsPlaying)
                IsPlaying = false;
            else
                throw new InvalidOperationException("Was already stopped.");
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