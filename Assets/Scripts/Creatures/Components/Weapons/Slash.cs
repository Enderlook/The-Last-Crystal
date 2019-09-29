using System;
using UnityEngine;

namespace CreaturesAddons
{
    public class Slash : Weapon
    {
        [SerializeField, Tooltip("Damage on hit.")]
        private float damage = 1;
        [SerializeField, Tooltip("Push strength on hit.")]
        private float pushStrength = 0;

        [Header("Setup")]
#pragma warning disable CS0649
        [SerializeField]
        private RayCasting rayCasting;
        [SerializeField]
        private LayerMask toHit;
#pragma warning restore CS0649

        Transform thisTransform;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Type Safety", "UNT0006:Incorrect message signature", Justification = "This isn't Unity method.")]
        public override void Init(Creature creature)
        {
            thisTransform = creature.Transform;
            base.Init(creature);
        }

        protected override void Attack()
        {
            RaycastHit2D raycastHit = rayCasting.Raycast(toHit.value);
            if (raycastHit.transform == null)
                return;
            Creature victim = raycastHit.transform.GetComponent<Creature>();
            if (victim == null)
                return;
            victim.TakeDamage(damage);
            victim.Push(thisTransform.position, pushStrength);
        }
    }
}