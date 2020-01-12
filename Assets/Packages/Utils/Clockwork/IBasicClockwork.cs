using UnityEngine;

namespace Additions.Utils.Clockworks
{
    public interface IBasicClockwork : IUpdate
    {
        /// <summary>
        /// Current cooldown time.
        /// </summary>
        float CooldownTime { get; }

        /// <summary>
        /// Total cooldown time.
        /// </summary>
        float TotalCooldown { get; }

        /// <summary>
        /// Cooldown percent from 0 to 1. When 0, it's ready to execute.
        /// </summary>
        float CooldownPercent { get; }

        /// <summary>
        /// Whenever it's ready or is still in cooldown.
        /// </summary>
        bool IsReady { get; }

        /// <summary>
        /// Reset <see cref="CooldownTime"/> time to <see cref="TotalCooldown"/>.
        /// </summary>
        void ResetCooldown();

        /// <summary>
        /// Assign a new maximum value <paramref name="newCooldownTime"/> and calls <see cref="ResetCooldown"/>.
        /// </summary>
        void ResetCooldown(float newCooldownTime);

        /// <summary>
        /// Reduce <see cref="CooldownTime"/> time and checks if the <see cref="CooldownTime"/> is over.
        /// </summary>
        /// <param name="deltaTime"><see cref="Time.deltaTime"/></param>
        /// <returns><see cref="IsReady"/>.</returns>
        bool Recharge(float deltaTime);
    }
}