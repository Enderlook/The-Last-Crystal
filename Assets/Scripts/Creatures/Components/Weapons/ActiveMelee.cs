using Additions.Attributes;
using Additions.Components.ScriptableSound;
using Additions.Utils;
using Additions.Utils.Clockworks;

using Creatures.Weapons;

using UnityEngine;

namespace Creatures
{
    public class ActiveMelee : MonoBehaviour, IInit, IUpdate, IBasicClockwork, IDamageOnTouch
    {
        [Header("Configuration")]
        [SerializeField, Tooltip("Damage on hit.")]
        private float damage = 1;

        [SerializeField, Tooltip("Push strength on hit.")]
        private float pushStrength = 0;

        [SerializeField, Tooltip("Attacks per second.")]
        private float firerate = 1;

        [field: SerializeField, Tooltip("Whenever it's enabled or disabled."), IsProperty]
        public bool IsEnabled { get; set; } = true;

#pragma warning disable CS0649
        [SerializeField, Tooltip("Sound played on hit.")]
        private SoundPlay hitSound;
#pragma warning restore CS0649

        private BasicClockwork basicClockwork;

        private Transform thisTransform;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Type Safety", "UNT0006:Incorrect message signature", Justification = "This isn't Unity method.")]
        void IInit.Init(Creature creature)
        {
            basicClockwork = new BasicClockwork(1 / firerate);
            thisTransform = creature.Transform;
            if (hitSound != null)
                hitSound.Init();
        }

        void IDamageOnTouch.ProduceDamage(object victim)
        {
            if (basicClockwork.IsReady && IsEnabled)
            {
                if (thisTransform != null && victim is IPush push)
                    push.Push(thisTransform.position, pushStrength, PushMode.Local);
                if (victim is ITakeDamage takeDamage)
                    takeDamage.TakeDamage(damage);
                if (hitSound != null)
                    hitSound.Play();
            }
        }

        public float CooldownTime => ((IBasicClockwork)basicClockwork).CooldownTime;
        public float TotalCooldown => ((IBasicClockwork)basicClockwork).TotalCooldown;
        public float CooldownPercent => ((IBasicClockwork)basicClockwork).CooldownPercent;
        public bool IsReady => ((IBasicClockwork)basicClockwork).IsReady && IsEnabled;

        public bool Recharge(float deltaTime)
        {
            if (IsEnabled)
                return ((IBasicClockwork)basicClockwork).Recharge(deltaTime);
            return IsReady;
        }

        public void ResetCooldown()
        {
            if (IsEnabled)
                ((IBasicClockwork)basicClockwork).ResetCooldown();
        }

        public void UpdateBehaviour(float deltaTime)
        {
            if (IsEnabled)
                ((IBasicClockwork)basicClockwork).UpdateBehaviour(deltaTime);
            if (hitSound != null)
                hitSound.UpdateBehaviour(deltaTime);
        }

        public void ResetCooldown(float newCooldownTime)
        {
            if (IsEnabled)
                ((IBasicClockwork)basicClockwork).ResetCooldown(newCooldownTime);
        }

        public void SetReady()
        {
            if (IsEnabled)
                ((IBasicClockwork)basicClockwork).SetReady();
        }
    }
}