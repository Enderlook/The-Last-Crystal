using UnityEngine;

namespace CreaturesAddons
{
    public class ActiveMelee : MonoBehaviour, IInit, IBasicClockWork, IDamageOnTouch
    {
        [Header("Configuration")]
        [SerializeField, Tooltip("Damage on hit.")]
        private float damage = 1;
        [SerializeField, Tooltip("Push strength on hit.")]
        private float pushStrength = 0;
        [SerializeField, Tooltip("Attacks per second.")]
        private float firerate = 1;

        private BasicClockwork basicClockwork;

        private Transform thisTransform;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Type Safety", "UNT0006:Incorrect message signature", Justification = "This isn't Unity method.")]
        void IInit.Init(Creature creature)
        {
            basicClockwork = new BasicClockwork(1 / firerate);
            thisTransform = creature.Transform;
        }

        void IDamageOnTouch.ProduceDamage(object victim)
        {
            if (basicClockwork.IsReady)
            {
                if (victim is IPush push)
                    push.Push(thisTransform.position, pushStrength, PushMode.Local);
                if (victim is ITakeDamage takeDamage)
                    takeDamage.TakeDamage(damage);
            }
        }

        public float CooldownTime => ((IBasicClockWork)basicClockwork).CooldownTime;
        public float TotalCooldown => ((IBasicClockWork)basicClockwork).TotalCooldown;
        public float CooldownPercent => ((IBasicClockWork)basicClockwork).CooldownPercent;
        public bool IsReady => ((IBasicClockWork)basicClockwork).IsReady;
        public bool Recharge(float deltaTime) => ((IBasicClockWork)basicClockwork).Recharge(deltaTime);
        public void ResetCooldown() => ((IBasicClockWork)basicClockwork).ResetCooldown();
        public void UpdateBehaviour(float deltaTime) => ((IBasicClockWork)basicClockwork).UpdateBehaviour(deltaTime);
        public void ResetCooldown(float newCooldownTime) => ((IBasicClockWork)basicClockwork).ResetCooldown(newCooldownTime);
    }
}