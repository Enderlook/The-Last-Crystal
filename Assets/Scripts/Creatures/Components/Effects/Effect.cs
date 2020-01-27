using Additions.Extensions;

using System;

using UnityEngine;

namespace Creatures.Effects
{
    public abstract class Effect<T>
    {
        private float endAt;
        private float lastUpdateAt;
        protected float duration;

        public bool shouldBeDisposed;
        protected T host;
        private IUpdateEffect update;

        /// <summary>
        /// Get duration percent.
        /// </summary>
        public float DurationPercent {
            get {
                try
                {
                    return (duration - RemainingDuration) / duration;
                }
                catch (DivideByZeroException)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Get the remaining duration.
        /// </summary>
        public float RemainingDuration => endAt - lastUpdateAt;

        /// <summary>
        /// Whenever the old instance of this effect should be removed before add this one.
        /// </summary>
        public abstract bool ReplaceCurrentInstance { get; }

        /// <summary>
        /// Construct the <see cref="Effect{T}"/>.
        /// </summary>
        /// <param name="duration">Duration in seconds of the effect.</param>
        protected Effect(float duration) => this.duration = duration;

        /// <summary>
        /// Initialize the effect.
        /// </summary>
        /// <param name="host">Instance of the <seealso cref="T"/> where it's being constructed.</param>
        /// <param name="totalTime">Total time (<see cref="Time.time"/>).</param>
        public void Setup(T host, float totalTime)
        {
            this.host = host;
            this.CastOrNull<IStartEffect>()?.OnStart();
            if (duration <= 0)
                End(false);
            this.TryCast(out update);
            endAt = totalTime + duration;
        }

        private void End(bool interrupted)
        {
            this.CastOrNull<IEndEffect>()?.OnEnd(interrupted);
            shouldBeDisposed = true;
        }

        /// <summary>
        /// Update the effect and reduce duration. Also determines if it <seealso cref="shouldBeDisposed"/>.
        /// </summary>
        /// <param name="time">Time in seconds since last frame (<seealso cref="Time.deltaTime"/>).</param>
        /// <param name="totalTime">Total time (<see cref="Time.time"/>).</param>
        public void Update(float time, float totalTime)
        {
            if (totalTime >= endAt)
                End(false);
            else
            {
                update?.OnUpdate(time);
                duration -= time;
            }
            lastUpdateAt = totalTime;
        }

        /// <summary>
        /// Force the interruption of the effect.
        /// </summary>
        public void Stop() => End(true);
    }
}