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
        //[SerializeField]
        public RayCasting rayCasting;
        [Layer]
        public int layerToHit;
#pragma warning restore CS0649

        private Transform thisTransform;
        private Animator thisAnimator;

        private static class ANIMATION_STATES
        {
            public const string
                ATTACK = "Attack";
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Type Safety", "UNT0006:Incorrect message signature", Justification = "This isn't Unity method.")]
        public override void Init(Creature creature)
        {
            thisTransform = creature.Transform;
            thisAnimator = creature.animator;
            base.Init(creature);
        }

        protected override void Attack()
        {
            thisAnimator.SetTrigger(ANIMATION_STATES.ATTACK);
            HitTarget();
        }

        private void HitTarget()
        {
            RaycastHit2D[] raycastHits = rayCasting.RaycastAll(1 << layerToHit); // Ignore any layer that isn't layerToHit
            for (int n = 0; n < raycastHits.Length; n++)
            {
                Transform victim = raycastHits[n].transform;
                victim.transform.GetComponent<IPush>()?.Push(thisTransform.position, pushStrength);
                victim.transform.GetComponent<ITakeDamage>()?.TakeDamage(damage);
            }
        }
    }
}