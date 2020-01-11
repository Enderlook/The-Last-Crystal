using UnityEngine;

namespace Additions.Utils.Clockworks
{
    public interface IClockwork : IBasicClockwork
    {
        /// <summary>
        /// Execute <see cref="Callback"/> and call <see cref="ResetCooldown"/>.<br/>
        /// It ignores the <see cref="IsReady"/>. Use <seealso cref="TryExecute(float)"/> to use it.
        /// </summary>
        /// <seealso cref="TryExecute(float)"/>
        void Execute();

        /// <summary>
        /// Try to execute <see cref="Callback"/>. It will check for the <see cref="CooldownTime"/>, and if possible, execute.
        /// </summary>
        /// <param name="deltaTime">Time since the last frame. <see cref="Time.deltaTime"/></param>
        /// <returns><see langword="true"/> if it was executed, <see langword="false"/> if it's still on cooldown.</returns>
        /// <seealso cref="Execute"/>
        bool TryExecute(float deltaTime = 0);

        /// <summary>
        /// Total number of times <see cref="Execute"/> can be called. -1 is unlimited.
        /// </summary>
        int TotalCycles { get; }

        /// <summary>
        /// Remaining number of times <see cref="Execute"/> can be called.
        /// </summary>
        int RemainingCycles { get; }

        /// <summary>
        /// Whenever there is no number of time <see cref="Execute"/> can be called.
        /// </summary>
        bool IsEndlessLoop { get; }

        /// <summary>
        /// Whenever the timer is working or not. If <see cref="RemainingCycles"/> is 0 the timer stop working.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Reset <see cref="RemainingCycles"/> to <see cref="TotalCycles"/>.
        /// </summary>
        void ResetCycles();

        /// <summary>
        /// Assign a new maximum value <paramref name="TotalCycles"/> and <see cref="RemainingCycles"/>.
        /// </summary>
        void ResetCycles(int newCycles);
    }
}