using UnityEngine;

namespace CreaturesAddons
{
    public class Slash : Weapon, IAutomatedAttack
    {
        [SerializeField, Tooltip("Damage on hit.")]
        private float damage = 1;
        [SerializeField, Tooltip("Push strength on hit.")]
        private float pushStrength = 0;

        [Header("Setup")]
#pragma warning disable CS0649
        [SerializeField]
        private RayCasting rayCasting;
        [Layer]
        private int layerToHit;
        [SerializeField, Tooltip("Animation played on attack.")]
        protected string animationState;
#pragma warning restore CS0649

        private Transform thisTransform;
        protected Animator thisAnimator;
        private SpriteRenderer thisSpriteRenderer;

        public bool TargetInRange => rayCasting.Raycast(1 << layerToHit);
        public bool AutoAttack { get; set; }

        public bool AttackIfIsReadyAndIfTargetInRange(float deltaTime = 0) => TargetInRange ? TryExecute(deltaTime) : Recharge(deltaTime);

        public override void Init(Creature creature)
        {
            thisTransform = creature.Transform;
            thisAnimator = creature.animator;
            thisSpriteRenderer = creature.sprite;
            rayCasting.SetReference(thisTransform, thisSpriteRenderer);
            base.Init(creature);
        }

        protected override void Attack()
        {
            if (thisAnimator == null || string.IsNullOrEmpty(animationState))
                HitTarget();
            else
                thisAnimator.SetTrigger(animationState);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Calidad del código", "IDE0051:Quitar miembros privados no utilizados", Justification = "Used by Unity Animator event 'Attack'")]
        protected void HitTarget()
        {
            RaycastHit2D[] raycastHits = rayCasting.RaycastAll(1 << layerToHit); // Ignore any layer that isn't layerToHit
            for (int n = 0; n < raycastHits.Length; n++)
            {
                Transform victim = raycastHits[n].transform;
                victim.transform.GetComponent<IPush>()?.Push(thisTransform.position, pushStrength);
                victim.transform.GetComponent<ITakeDamage>()?.TakeDamage(damage);
            }
        }

        private void AttackIfAutomated()
        {
            if (AutoAttack)
                AttackIfIsReadyAndIfTargetInRange();
        }

        public override bool Recharge(float deltaTime)
        {
            bool value = base.Recharge(deltaTime);
            AttackIfAutomated();
            return value;
        }

        public override void UpdateBehaviour(float deltaTime)
        {
            base.UpdateBehaviour(deltaTime);
            AttackIfAutomated();
        }
    }
}