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

        public virtual void ProduceDamage(Creature victim)
        {
            victim.TakeDamage(damage);
            victim.Push(thisTransform.position, pushStrength, Creature.PushMode.Local);
        }
    }

    public interface IDamageOnTouch
    {
        /// <summary>
        /// Produce damage
        /// </summary>
        /// <param name="creature"></param>
        void ProduceDamage(Creature victim);
    }
}