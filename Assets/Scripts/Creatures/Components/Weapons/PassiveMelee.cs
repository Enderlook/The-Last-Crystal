using System;
using UnityEngine;

namespace CreaturesAddons.Weapons
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

        protected Transform thisTransform;

        void IInit.Init(Creature creature) => thisTransform = creature.Transform;

        public virtual void ProduceDamage(object victim)
        {
            if (thisTransform != null && victim is IPush push)
                push.Push(thisTransform.position, pushStrength, PushMode.Local);
            if (victim is ITakeDamage takeDamage)
                takeDamage.TakeDamage(damage, showHurt, showHurt);
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