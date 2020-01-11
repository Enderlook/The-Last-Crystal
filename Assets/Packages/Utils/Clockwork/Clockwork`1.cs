using System;

namespace Additions.Utils.Clockworks
{
    public class Clockwork<T> : Clockwork, IClockwork<T>
    {
        private Func<T> Callback;

        /// <summary>
        /// Create a timer that executes <paramref name="Callback"/> each <paramref name="cooldown"/> seconds.<br/>
        /// Time must be manually updated using <see cref="Recharge(float)"/>, <see cref="TryExecute(float)"/> or <see cref="TryExecute(ref T, float)"/> methods.
        /// </summary>
        /// <param name="cooldown">Time in seconds to execute <paramref name="Callback"/>.</param>
        /// <param name="Callback">Action to execute.</param>
        /// <param name="autoExecute">Whenever <see cref="Update(float)"/> must call <see cref="Execute"/> when <see cref="CooldownTime"/> is 0.</param>
        public Clockwork(float cooldown, Func<T> Callback, bool autoExecute) : base(cooldown, () => Callback(), autoExecute)
        {
            TotalCooldown = cooldown;
            ResetCooldown();
            this.Callback = Callback;
        }

        public bool TryExecute(ref T result, float deltaTime = 0)
        {
            if (Recharge(deltaTime))
            {
                result = Execute();
                return true;
            }
            return false;
        }

        public new T Execute()
        {
            ResetCooldown();
            return Callback();
        }
    }
}