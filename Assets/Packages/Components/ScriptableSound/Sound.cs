using AdditionalAttributes;

using System;

using UnityEngine;
using Utils;

namespace ScriptableSound
{
    [AbstractScriptableObject]
    public class Sound : ScriptableObject, IPrototypable<Sound>
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
        protected bool ShouldChangeSound => IsPlaying && soundConfiguration != null && soundConfiguration.audioSource.time == 0; // TODO: soundConfiguration shouldn't be null if IsPlay is true, but removing this produces issues

        /// <summary>
        /// Updates behavior.
        /// </summary>
        /// <param name="deltaTime">Time since last update in seconds. <seealso cref="Time.deltaTime"/></param>
        public virtual void UpdateBehaviour(float deltaTime) => throw new NotImplementedException("This class is 'abstract' and should not be instantiated by its own. Use derived classes instead which override this method.");

        /// <summary>
        /// Instantiate a prototype of this instance using this as a template.
        /// </summary>
        /// <returns>New instance based on this one as template.</returns>
        public virtual Sound CreatePrototype() => throw new NotImplementedException("This class is 'abstract' and should not be instantiated by its own. Use derived classes instead which override this method.");
    }
}