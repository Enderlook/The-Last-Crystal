using AdditionalAttributes;

using Master;

using Serializables.Physics;

using UnityEngine;

namespace CreaturesAddons.Weapons
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
        [SerializeField, Layer, Tooltip("The target to hit")]
        private int layerToHit;
        [SerializeField, Tooltip("Animation played on attack.")]
        protected string animationState;
#pragma warning restore CS0649

        private Transform thisTransform;
        protected Animator thisAnimator;
        private SpriteRenderer thisSpriteRenderer;
        private int countOfClicks;

        private static class ANIMATION_STATES
        {
            public const string
                SECOND_COMBO = "Attack2",
                THIRD_COMBO = "Attack3";
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
            countOfClicks++;
            if (thisAnimator == null || string.IsNullOrEmpty(animationState))
                HitTarget();
            else if (countOfClicks == 1)
                thisAnimator.SetBool(animationState, true);
            countOfClicks = Mathf.Clamp(countOfClicks, 0, 3);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Calidad del código", "IDE0051:Quitar miembros privados no utilizados", Justification = "Used by Unity Animator event 'Attack'")]
        protected void HitTarget()
        {
            Global.audioSystem.SlashSoundEffect();
            RaycastHit2D[] raycastHits = rayCasting.RaycastAll(1 << layerToHit); // Ignore any layer that isn't layerToHit
            for (int n = 0; n < raycastHits.Length; n++)
            {
                Transform victim = raycastHits[n].transform;
                victim.transform.GetComponent<IPush>()?.Push(thisTransform.position, pushStrength);
                victim.transform.GetComponent<ITakeDamage>()?.TakeDamage(damage);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Calidad del código", "IDE0051:Quitar miembros privados no utilizados", Justification = "Used by Unity Animator event 'Attack'")]
        protected void ComboHitA()
        {
            if (countOfClicks >= 2)
                thisAnimator.SetBool(ANIMATION_STATES.SECOND_COMBO, true);
            else
            {
                thisAnimator.SetBool(animationState, false);
                countOfClicks = 0;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Calidad del código", "IDE0051:Quitar miembros privados no utilizados", Justification = "Used by Unity Animator event 'Attack2'")]
        protected void ComboHitB()
        {
            if (countOfClicks >= 3)
                thisAnimator.SetBool(ANIMATION_STATES.THIRD_COMBO, true);
            else
            {
                thisAnimator.SetBool(ANIMATION_STATES.SECOND_COMBO, false);
                countOfClicks = 0;
            }
        }

        protected void ResetAnimation(int isCombo = 0)
        {
            if (isCombo == 1)
            {
                thisAnimator.SetBool(animationState, false);
                thisAnimator.SetBool(ANIMATION_STATES.SECOND_COMBO, false);
                thisAnimator.SetBool(ANIMATION_STATES.THIRD_COMBO, false);
            }
            else if (isCombo == 0)
                thisAnimator.SetBool(animationState, false);
            countOfClicks = 0;
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