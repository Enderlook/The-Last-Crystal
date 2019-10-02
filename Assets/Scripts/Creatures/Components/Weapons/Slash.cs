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
        [SerializeField, Layer]
        private int layerToHit;
#pragma warning restore CS0649

        private Transform thisTransform;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Type Safety", "UNT0006:Incorrect message signature", Justification = "This isn't Unity method.")]
        public override void Init(Creature creature)
        {
            thisTransform = creature.Transform;
            base.Init(creature);
        }

        protected override void Attack()
        {
            RaycastHit2D raycastHit = rayCasting.Raycast(1 << layerToHit); // Ignore any layer that isn't layerToHit
            if (raycastHit.transform == null)
                return;

            Transform victim = raycastHit.transform;
            victim.GetComponent<ITakeDamage>()?.TakeDamage(damage);
            victim.GetComponent<IPush>()?.Push(thisTransform.position, pushStrength);
        }
    }
}