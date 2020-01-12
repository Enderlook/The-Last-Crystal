using UnityEngine;

namespace Additions.Utils.Clockworks
{
    public class BasicClockwork : IBasicClockwork
    {
        public float CooldownTime {
            get => cooldownTime;
            private set {
                cooldownTime = value;
                if (cooldownTime < 0)
                    cooldownTime = 0;
            }
        }

        protected float cooldownTime;

        public float TotalCooldown { get; protected set; }

        public float CooldownPercent => Mathf.Clamp01(CooldownTime / TotalCooldown);

        public bool IsReady => CooldownTime <= 0;

        /// <summary>
        /// Create a timer.<br/>
        /// Time must be manually updated using <see cref="Recharge(float)"/>.
        /// </summary>
        /// <param name="cooldown">Time in seconds to execute <paramref name="Callback"/>.</param>
        public BasicClockwork(float cooldown)
        {
            TotalCooldown = cooldown;
            ResetCooldown();
        }

        public void ResetCooldown() => CooldownTime = TotalCooldown;

        public void ResetCooldown(float newCooldownTime)
        {
            TotalCooldown = newCooldownTime;
            ResetCooldown();
        }

        public bool Recharge(float deltaTime)
        {
            CooldownTime -= deltaTime;
            return IsReady;
        }

        /// <summary>
        /// Calls <see cref="Recharge(float)"/>.</summary>
        /// <param name="deltaTime">Time since last increase.</param>
        public void UpdateBehaviour(float deltaTime) => Recharge(deltaTime);
    }
}