using UnityEngine;

namespace CreaturesAddons
{
    public abstract class Weapon : MonoBehaviour, IClockWork
    {
        [Header("Configuration")]
        [Tooltip("Attacks per second.")]
        public float firerate;

        private Clockwork clockwork;

        private void Awake()
        {
            clockwork = new Clockwork(1 / firerate, Attack, false);
        }

        protected abstract void Attack();

        public float CooldownTime => ((IClockWork)clockwork).CooldownTime;
        public float TotalCooldown => ((IClockWork)clockwork).TotalCooldown;
        public float CooldownPercent => ((IClockWork)clockwork).CooldownPercent;
        public bool IsReady => ((IClockWork)clockwork).IsReady;
        public void Execute() => ((IClockWork)clockwork).Execute();
        public bool Recharge(float deltaTime) => ((IClockWork)clockwork).Recharge(deltaTime);
        public void ResetCooldown() => ((IClockWork)clockwork).ResetCooldown();
        public bool TryExecute(float deltaTime = 0) => ((IClockWork)clockwork).TryExecute(deltaTime);
        public void Update(float deltaTime) => ((IClockWork)clockwork).Update(deltaTime);
    }
}