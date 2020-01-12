
using Additions.Utils.Clockworks;

using UnityEngine;

namespace CreaturesAddons.Weapons
{
    public abstract class Weapon : MonoBehaviour, IInit, IClockwork
    {
        [Header("Configuration")]
        [SerializeField, Tooltip("Attacks per second.")]
        private float firerate = 1;

        private Clockwork clockwork;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Type Safety", "UNT0006:Incorrect message signature", Justification = "This isn't Unity method.")]
        public virtual void Init(Creature creature) => clockwork = new Clockwork(1 / firerate, Attack, false);

        protected abstract void Attack();

        public float CooldownTime => ((IClockwork)clockwork).CooldownTime;
        public float TotalCooldown => ((IClockwork)clockwork).TotalCooldown;
        public float CooldownPercent => ((IClockwork)clockwork).CooldownPercent;
        public bool IsReady => ((IClockwork)clockwork).IsReady;
        public int TotalCycles => ((IClockwork)clockwork).TotalCycles;
        public int RemainingCycles => ((IClockwork)clockwork).RemainingCycles;
        public bool IsEndlessLoop => ((IClockwork)clockwork).IsEndlessLoop;
        public bool IsEnabled => ((IClockwork)clockwork).IsEnabled;
        public void Execute() => ((IClockwork)clockwork).Execute();
        public virtual bool Recharge(float deltaTime) => ((IClockwork)clockwork).Recharge(deltaTime);
        public void ResetCooldown() => ((IClockwork)clockwork).ResetCooldown();
        public bool TryExecute(float deltaTime = 0) => ((IClockwork)clockwork).TryExecute(deltaTime);
        public virtual void UpdateBehaviour(float deltaTime) => ((IClockwork)clockwork).UpdateBehaviour(deltaTime);
        public void ResetCycles() => ((IClockwork)clockwork).ResetCycles();
        public void ResetCycles(int newCycles) => ((IClockwork)clockwork).ResetCycles(newCycles);
        public void ResetCooldown(float newCooldownTime) => ((IClockwork)clockwork).ResetCooldown(newCooldownTime);
    }

    public interface ITakeDamage
    {
        void TakeDamage(float amount, bool displayText = true, bool displayAnimation = true);
    }

    public interface IPush
    {
        void Push(Vector2 direction, float force = 1, PushMode pushMode = PushMode.Local);
    }

    public enum PushMode { Local, Global };

    public interface IAutomatedAttack
    {
        /// <summary>
        /// Whenever the weapon has any target in range or not.
        /// </summary>
        bool TargetInRange { get; }

        /// <summary>
        /// Check if <see cref="TargetInRange"/>.<br>
        /// If <see langword="true"/> calls <see cref="Weapon.TryExecute(float)"/> using <paramref name="deltaTime"/> as parameter.<br>
        /// If <see langword="false"/> calls <see cref="Weapon.Recharge(float)"/> using <paramref name="deltaTime"/>.
        /// </summary>
        /// <returns>Whenever an attack was made or not.</returns>
        bool AttackIfIsReadyAndIfTargetInRange(float deltaTime = 0);

        /// <summary>
        /// Whenever should automatically attack or not when <see cref="TargetInRange"/> and <see cref="Weapon.IsReady"/> are <see langword="true"/> on each <see cref="Weapon.UpdateBehaviour(float)"/> or <see cref="Weapon.Recharge(float)"/> call.
        /// </summary>
        bool AutoAttack { get; set; }
    }
}