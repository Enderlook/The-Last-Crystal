using UnityEngine;

namespace Additions.Utils.Clockworks
{
    public interface IClockwork<T> : IClockwork
    {
        /// <summary>
        /// Execute <see cref="Callback"/> and call <see cref="ResetCooldown"/>.<br/>
        /// It ignores the <see cref="IsReady"/>. Use <seealso cref="TryExecute(float)"/> to use it.
        /// </summary>
        /// <returns>The result of <see cref="Callback"/>.</returns>
        /// <seealso cref="TryExecute(float)"/>
        new T Execute();

        /// <summary>
        /// Try to execute <see cref="Callback"/>. It will check for the <see cref="CooldownTime"/>, and if possible, execute.
        /// </summary>
        /// <param name="deltaTime">Time since the last frame. <see cref="Time.deltaTime"/></param>
        /// <param name="result">The result of <see cref="Callback"/>.</param>
        /// <returns><see langword="true"/> if it was executed, <see langword="false"/> if it's still on cooldown.</returns>
        /// <seealso cref="Execute"/>
        bool TryExecute(ref T result, float deltaTime = 0);
    }
}