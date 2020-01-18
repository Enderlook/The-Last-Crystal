using Additions.Utils;
using Additions.Utils.Clockworks;

using UnityEngine;

namespace Creatures.Weapons
{
    public abstract class Weapon : MonoBehaviour, IInitialize<Creature>, IClockwork
    {
        [Header("Configuration")]
        [SerializeField, Tooltip("Attacks per second.")]
        private float firerate = 1;

        private Clockwork clockwork;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Type Safety", "UNT0006:Incorrect message signature", Justification = "This isn't Unity method.")]
        public virtual void Initialize(Creature creature) => clockwork = new Clockwork(1 / firerate, Attack, false);

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
        public void SetReady() => ((IClockwork)clockwork).SetReady();
    }
}