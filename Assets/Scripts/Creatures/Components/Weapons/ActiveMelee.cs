
using Additions.Utils.Clockworks;

using CreaturesAddons.Weapons;

using UnityEngine;

namespace CreaturesAddons
{
    public class ActiveMelee : MonoBehaviour, IInit, IBasicClockwork, IDamageOnTouch
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
                if (thisTransform != null && victim is IPush push)
                    push.Push(thisTransform.position, pushStrength, PushMode.Local);
                if (victim is ITakeDamage takeDamage)
                    takeDamage.TakeDamage(damage);
            }
        }

        public float CooldownTime => ((IBasicClockwork)basicClockwork).CooldownTime;
        public float TotalCooldown => ((IBasicClockwork)basicClockwork).TotalCooldown;
        public float CooldownPercent => ((IBasicClockwork)basicClockwork).CooldownPercent;
        public bool IsReady => ((IBasicClockwork)basicClockwork).IsReady;
        public bool Recharge(float deltaTime) => ((IBasicClockwork)basicClockwork).Recharge(deltaTime);
        public void ResetCooldown() => ((IBasicClockwork)basicClockwork).ResetCooldown();
        public void UpdateBehaviour(float deltaTime) => ((IBasicClockwork)basicClockwork).UpdateBehaviour(deltaTime);
        public void ResetCooldown(float newCooldownTime) => ((IBasicClockwork)basicClockwork).ResetCooldown(newCooldownTime);
    }
}