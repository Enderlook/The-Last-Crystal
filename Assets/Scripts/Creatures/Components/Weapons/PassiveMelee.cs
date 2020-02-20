using Additions.Attributes;
using Additions.Serializables.Atoms;
using Additions.Serializables.Atoms.Premades.System;
using Additions.Utils;

using Creatures.Effects;

using UnityEngine;

namespace Creatures.Weapons
{
    public class PassiveMelee : MonoBehaviour, IInitialize<Creature>, IDamageOnTouch<Creature>
    {
#pragma warning disable CS0649
        [Header("Configuration")]
        [SerializeField, Tooltip("Damage on hit."), Expandable, RestrictType(typeof(IGet<float>))]
        private Atom damage;
#pragma warning restore CS0649

        protected IGet<float> _damage;

        protected float Damage {
            get {
                if (_damage == null)
                    _damage = (IGet<float>)damage;
                return _damage.Value;
            }
            set => _damage = FloatConstant.Create<FloatConstant>(value);
        }

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
                takeDamage?.TakeDamage(Damage, showHurt, showHurt);
            }
        }
    }
}