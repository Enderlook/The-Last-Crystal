using Additions.Attributes;
using UnityEngine;

namespace Creatures.Weapons
{
    public class PassiveMelee : MonoBehaviour, IInit, IDamageOnTouch
    {
        [Header("Configuration")]
        [SerializeField, Tooltip("Damage on hit.")]
        protected float damage = 1;

        [SerializeField, Tooltip("Push strength on hit.")]
        protected float pushStrength = 0;

        [SerializeField, Tooltip("Whenever it should or not display damage floating text and animation.")]
        private bool showHurt = true;

        [field: SerializeField, Tooltip("Whenever it's enabled or disabled."), IsProperty]
        public bool IsEnabled { get; set; } = true;

        protected Transform thisTransform;

        void IInit.Init(Creature creature) => thisTransform = creature.Transform;

        public virtual void ProduceDamage(object victim)
        {
            if (IsEnabled)
            {
                if (thisTransform != null && victim is IPush push)
                    push.Push(thisTransform.position, pushStrength, PushMode.Local);
                if (victim is ITakeDamage takeDamage)
                    takeDamage.TakeDamage(damage, showHurt, showHurt);
            }
        }
    }

    public interface IDamageOnTouch
    {
        /// <summary>
        /// Produce damage. It will try to cast <paramref name="victim"/> to <see cref="ITakeDamage"/> and <see cref="IPush"/>.
        /// </summary>
        /// <param name="victim">Victim who will receive damage and possibly be pushed back.</param>
        void ProduceDamage(object victim);
    }
}