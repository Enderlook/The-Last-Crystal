using UnityEngine;

namespace CreaturesAddons
{
    public abstract class Weapon : MonoBehaviour, IAwake, IClockWork
    {
        [Header("Configuration")]
        [SerializeField, Tooltip("Attacks per second.")]
        private float firerate = 1;

        private Clockwork clockwork;


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Type Safety", "UNT0006:Incorrect message signature", Justification = "This isn't Unity method.")]
        public virtual void Awake(Creature creature) => clockwork = new Clockwork(1 / firerate, Attack, false);

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
        public void Update(float deltaTime) => ((IClockWork)clockwork).Update(deltaTime);
    }
}