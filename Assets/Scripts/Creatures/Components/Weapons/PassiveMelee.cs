using Additions.Attributes;
using Additions.Utils;

using Creatures.Effects;

using UnityEngine;

namespace Creatures.Weapons
{
    public class PassiveMelee : MonoBehaviour, IInitialize<Creature>, IDamageOnTouch<Creature>
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

        public virtual void Initialize(Creature creature) => thisTransform = creature.Transform;

        public virtual void ProduceDamage(IHasHealth takeDamage, ITakePush takePush, ITakeEffect<Creature> takeEffect)
        {
            if (IsEnabled)
            {
                if (thisTransform != null)
                    takePush?.TakePush(thisTransform.position, pushStrength, PushMode.Local);
                takeDamage?.TakeDamage(damage, showHurt, showHurt);
            }
        }
    }
}