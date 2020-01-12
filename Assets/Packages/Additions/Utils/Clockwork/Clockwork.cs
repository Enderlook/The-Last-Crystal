using System;

using UnityEngine;

namespace Additions.Utils.Clockworks
{
    public class Clockwork : IClockwork
    {
        private Action Callback;

        private bool autoExecute;

        public float CooldownTime {
            get => cooldownTime;
            private set {
                cooldownTime = value;
                if (cooldownTime < 0)
                    cooldownTime = 0;
            }
        }

        protected float cooldownTime = 0f;

        public float TotalCooldown { get; protected set; }

        public float CooldownPercent => Mathf.Clamp01(CooldownTime / TotalCooldown);

        public bool IsReady => CooldownTime <= 0;

        public int TotalCycles { get; private set; }

        public int RemainingCycles { get; private set; }

        public bool IsEndlessLoop => TotalCycles == -1;

        public bool IsEnabled => RemainingCycles > 0 || IsEndlessLoop;

        /// <summary>
        /// Create a timer that executes <paramref name="Callback"/> each <paramref name="cooldown"/> seconds.<br/>
        /// Time must be manually updated using <see cref="Recharge(float)"/>, <see cref="TryExecute(float)"/> or <see cref="TryExecute(ref T, float)"/> methods.
        /// </summary>
        /// <param name="cooldown">Time in seconds to execute <paramref name="Callback"/>.</param>
        /// <param name="Callback">Action to execute.</param>
        /// <param name="autoExecute">Whenever <see cref="UpdateBehaviour(float)"/> must call <see cref="Execute"/> when <see cref="CooldownTime"/> is 0.</param>
        /// <param name="cycles">Number of times <see cref="Execute"/> can be call. Use -1 for unlimited. Use <see cref="ResetCycles"/> to recover their uses. Don't use 0 or the timer will be disabled by default.</param>
        public Clockwork(float cooldown, System.Action Callback, bool autoExecute = true, int cycles = -1)
        {
            ResetCycles(cycles);
            ResetCooldown(cooldown);
            this.Callback = Callback;
            this.autoExecute = autoExecute;
        }

        public void Execute()
        {
            if (ReduceCyclesByOne())
            {
                ResetCooldown();
                Callback();
            }
        }

        private bool ReduceCyclesByOne()
        {
            if (IsEndlessLoop)
                return true;
            bool enabled = IsEnabled;
            RemainingCycles--;
            return enabled;
        }

        public bool TryExecute(float deltaTime = 0)
        {
            if (IsEnabled && Recharge(deltaTime))
            {
                Execute();
                return true;
            }
            return false;
        }

        public void ResetCooldown() => CooldownTime = TotalCooldown;

        public void ResetCooldown(float newCooldownTime)
        {
            TotalCooldown = newCooldownTime;
            ResetCooldown();
        }

        public void ResetCycles() => RemainingCycles = TotalCycles;

        public void ResetCycles(int newCycles)
        {
            TotalCycles = newCycles;
            ResetCycles();
        }

        public bool Recharge(float deltaTime)
        {
            if (IsEnabled)
            {
                CooldownTime -= deltaTime;
                return IsReady;
            }
            return false;
        }

        /// <summary>
        /// Calls <see cref="Recharge(float)"/> if <see cref="IsEnabled"/>, and calls <see cref="Execute"/> if <see cref="autoExecute"/> is <see langword="true"/> and <see cref="Recharge(float)"/> returned <see langword="true"/>.
        /// </summary>
        /// <param name="deltaTime">Time since last increase.</param>
        public void UpdateBehaviour(float deltaTime)
        {
            if (IsEnabled && Recharge(deltaTime) && autoExecute)
                Execute();
        }
    }
}