using Additions.Attributes;
using Additions.Components.ScriptableSound;
using Additions.Serializables.Atoms;
using Additions.Serializables.Physics;

using UnityEngine;

namespace Creatures.Weapons
{
    public class Slash : Weapon, IAutomatedAttack
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Damage on hit."), Expandable, RestrictType(typeof(AtomGet<float>))]
        private Atom damage;
#pragma warning restore CS0649

        private float Damage;

        [SerializeField, Tooltip("Push strength on hit.")]
        private float pushStrength = 0;

#pragma warning disable CS0649
        [Header("Setup")]
        [SerializeField]
        private Raycaster rayCasting;

        [SerializeField, Layer, Tooltip("The target to hit")]
        private int layerToHit;

        [SerializeField, Tooltip("Animation played on attack.")]
        protected string animationState;

        [SerializeField, Tooltip("Slashing sound.")]
        private SoundPlay slashingSound;
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

        public bool TargetInRange => rayCasting.Raycast(1 << layerToHit).collider != null;
        public bool AutoAttack { get; set; }

        public bool AttackIfIsReadyAndIfTargetInRange(float deltaTime = 0) => TargetInRange ? TryExecute(deltaTime) : Recharge(deltaTime);

        public override void Initialize(Creature creature)
        {
            thisTransform = creature.Transform;
            thisAnimator = creature.Animator;
            thisSpriteRenderer = creature.Sprite;
            rayCasting.SetReference(thisTransform, thisSpriteRenderer);
            slashingSound.Init();
            if (damage != null)
                Damage = damage.GetValue<float>();
            base.Initialize(creature);
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity Animator.")]
        protected void HitTarget()
        {
            slashingSound.Play();
            RaycastHit2D[] raycastHits = rayCasting.RaycastAll(1 << layerToHit); // Ignore any layer that isn't layerToHit
            for (int n = 0; n < raycastHits.Length; n++)
            {
                Transform victim = raycastHits[n].transform;
                victim.transform.GetComponent<ITakePush>()?.TakePush(thisTransform.position, pushStrength);
                victim.transform.GetComponent<IHasHealth>()?.TakeDamage(Damage);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity Animator.")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity Animator.")]
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
            slashingSound.UpdateBehaviour(deltaTime);
            base.UpdateBehaviour(deltaTime);
            AttackIfAutomated();
        }
    }
}
