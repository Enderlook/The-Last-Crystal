using UnityEngine;

namespace CreaturesAddons
{
    public class PassiveMelee : MonoBehaviour, IInit, IDamageOnTouch
    {
        [Header("Configuration")]
        [SerializeField, Tooltip("Damage on hit.")]
        protected float damage = 1;
#pragma warning disable CS0649
        [SerializeField, Tooltip("Push strength on hit.")]
        protected float pushStrength;
#pragma warning restore CS0649

        private Transform thisTransform;

        void IInit.Init(Creature creature) => thisTransform = creature.Transform;

        public virtual void ProduceDamage(object victim)
        {
            if (victim is ITakeDamage takeDamage)
                takeDamage.TakeDamage(damage);
            if (victim is IPush push)
                push.Push(thisTransform.position, pushStrength, PushMode.Local);
        }
    }

    public interface IDamageOnTouch
    {
        /// <summary>
        /// Produce damage. It will try to cast it to <see cref="ITakeDamage"/> and <see cref="IPush"/>.
        /// </summary>
        /// <param name="creature"></param>
        void ProduceDamage(object victim);
    }
}