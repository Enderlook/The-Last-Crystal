using UnityEngine;

namespace CreaturesAddons
{
    public abstract class Weapon : MonoBehaviour, IInit, IClockWork
    {
        [Header("Configuration")]
        [SerializeField, Tooltip("Attacks per second.")]
        private float firerate = 1;

        private Clockwork clockwork;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Type Safety", "UNT0006:Incorrect message signature", Justification = "This isn't Unity method.")]
        public virtual void Init(Creature creature) => clockwork = new Clockwork(1 / firerate, Attack, false);

        protected abstract void Attack();

        public float CooldownTime => ((IClockWork)clockwork).CooldownTime;
        public float TotalCooldown => ((IClockWork)clockwork).TotalCooldown;
        public float CooldownPercent => ((IClockWork)clockwork).CooldownPercent;
        public bool IsReady => ((IClockWork)clockwork).IsReady;
        public void Execute() => ((IClockWork)clockwork).Execute();
        public bool Recharge(float deltaTime) => ((IClockWork)clockwork).Recharge(deltaTime);
        public void ResetCooldown() => ((IClockWork)clockwork).ResetCooldown();
        public bool TryExecute(float deltaTime = 0) => ((IClockWork)clockwork).TryExecute(deltaTime);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Type Safety", "UNT0006:Incorrect message signature", Justification = "This isn't Unity method.")]
        public void UpdateBehaviour(float deltaTime) => ((IClockWork)clockwork).UpdateBehaviour(deltaTime);
    }

    public interface ITakeDamage
    {
        void TakeDamage(float amount, bool displayText = false, bool displayAnimation = true);
    }

    public interface IPush
    {
        void Push(Vector2 direction, float force = 1, PushMode pushMode = PushMode.Local);
    }
    public enum PushMode { Local, Global };
}