using UnityEngine;

namespace CreaturesAddons
{
    public class Slash : Weapon, IAutomatedAttack
    {
        [SerializeField, Tooltip("Enemy pattern attack")]
        private bool activePattern;
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
        private SpriteRenderer thisSpriteRenderer;
        private int probStrongAttack = 1;

        private static class ANIMATION_STATES
        {
            public const string
                ATTACK = "Attack",
                STRONG_ATTACK = "StrongAttack";
        }

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
            if (activePattern)
            {
                if (probStrongAttack == Random.Range(0, 6))
                    thisAnimator.SetTrigger(ANIMATION_STATES.STRONG_ATTACK);
                else
                    thisAnimator.SetTrigger(ANIMATION_STATES.ATTACK);
            }
            else if (thisAnimator == null || string.IsNullOrEmpty(ANIMATION_STATES.ATTACK))
                HitTarget();
            else
                thisAnimator.SetTrigger(ANIMATION_STATES.ATTACK);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Calidad del código", "IDE0051:Quitar miembros privados no utilizados", Justification = "Used by Unity Animator event 'Attack'")]
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