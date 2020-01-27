using Additions.Utils;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Creatures.Effects
{
    public class EffectManager<T> : IUpdate
    {
        public List<Effect<T>> effects = new List<Effect<T>>();
        private T host;
        private float totalTime;

        /// <summary>
        /// Construct <see cref="EffectManager"/>.
        /// </summary>
        /// <param name="host">Instance of the <seealso cref="T"/> where it's being constructed.</param>
        public EffectManager(T host) => this.host = host;

        /// <summary>
        /// Add an effect to the creature and track it.
        /// </summary>
        /// <param name="effect">Effect to add.</param>
        public void AddEffect(Effect<T> effect)
        {
            // Single use effects don't replace current instance because they don't have
            if (effect.ReplaceCurrentInstance && effect.DurationPercent > 0)
            {
                Type target = effect.GetType();
                for (int i = 0; i < effects.Count; i++)
                {
                    if (effects[i].GetType() == target)
                    {
                        effects[i].Stop();
                        effects.RemoveAt(i);
                        break;
                    }
                }
            }
            effect.Setup(host, totalTime);
            // Some effect don't have duration and so they are disposed after Setup
            if (!effect.shouldBeDisposed)
                effects.Add(effect);
        }

        /// <summary>
        /// Update effects.
        /// </summary>
        /// <param name="deltaTime">Time in seconds since last frame (<seealso cref="Time.deltaTime"/>).</param>
        public void UpdateBehaviour(float deltaTime)
        {
            totalTime += deltaTime;
            for (int i = 0; i < effects.Count; i++)
            {
                Effect<T> effect = effects[i];
                if (effect.shouldBeDisposed)
                    effects.RemoveAt(i);
                else
                    effect.Update(deltaTime, totalTime);
            }
        }

        /// <summary>
        /// Check if has at least one instance of an effect of type <typeparamref name="U"/>.
        /// </summary>
        /// <typeparam name="U">Type of the effect to check.</typeparam>
        /// <returns>Whenever it has at least one instance of that effect type.</returns>
        public bool HasEffect<U>() where U : Effect<T> => effects.Any(e => e.GetType() == typeof(U));
    }
}