using UnityEngine;

namespace Creatures.Effects
{
    public interface IUpdateEffect
    {
        /// <summary>
        /// Update the effect.
        /// </summary>
        /// <param name="deltaTime">Time in seconds since last frame (<seealso cref="Time.deltaTime"/>).</param>
        void OnUpdate(float deltaTime);
    }
}