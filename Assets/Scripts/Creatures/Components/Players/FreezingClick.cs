using Additions.Attributes;
using Additions.Components.FloatPool;
using Additions.Components.ScriptableSound;
using Additions.Serializables.Atoms;
using Additions.Utils;

using Creatures.Effects;
using Creatures.Effects.Effects;

using UnityEngine;

namespace Creatures.Weapons
{
    public class FreezingClick : Weapon
    {
#pragma warning disable CS0649
        [Header("Configuration")]
        [SerializeField, Tooltip("Duration of freeze effect."), Expandable, RestrictType(typeof(AtomGet<float>))]
        private Atom effectDuration;

        private float EffectDuration;

        [SerializeField, Tooltip("Energy consumed to shoot.")]
        private float energyCost;

        [Header("Cloud Configuration")]
        [SerializeField, Tooltip("Whenever if projectile collider is trigger.")]
        private bool cloudColliderIsTrigger;

        [SerializeField, Tooltip("Effect range multiplier.")]
        private float cloudRangeMultiplier = 1;

        [SerializeField, Tooltip("The size of the collider is equal to the sprite size multiplicated by this value.")]
        private float cloudColliderMultiplier = 1;

        [SerializeField, Tooltip("Cloud duration.")]
        private float cloudDuration;

        [SerializeField, Tooltip("Projectile layer."), Layer]
        private int cloudLayer;

        [Header("Setup")]
        [SerializeField, Tooltip("Enemy effect color.")]
        private Color effectColor = Color.cyan;

        [SerializeField, Tooltip("Shooting sound.")]
        private SoundPlay shootingSound;

        [SerializeField, Tooltip("Animation attack name.")]
        private string animationName;

        [SerializeField, Tooltip("Animation played by effect.")]
        private RuntimeAnimatorController cloudAnimation;

        [SerializeField, Tooltip("Energy pool.")]
        private Pool energyPool;
#pragma warning restore CS0649

        private Transform shootingTransform;
        private Animator thisAnimator;
        private SpriteRenderer creatureSpriteRenderer;

        public override void Initialize(Creature creature)
        {
            shootingTransform = creature.Transform;
            thisAnimator = creature.Animator;
            creatureSpriteRenderer = creature.GetComponent<SpriteRenderer>();
            shootingSound.Init();
            EffectDuration = effectDuration.GetValue<float>();
            base.Initialize(creature);
        }

        public override void UpdateBehaviour(float deltaTime)
        {
            shootingSound.UpdateBehaviour(deltaTime);
            base.UpdateBehaviour(deltaTime);
        }

        protected override void Attack()
        {
            if (energyPool.Current >= energyCost)
            {
                energyPool.Decrease(energyCost);
                if (thisAnimator == null || string.IsNullOrEmpty(animationName))
                    Cast();
                else
                    thisAnimator.SetTrigger(animationName);
            }
        }

        private void Cast()
        {
            shootingSound.Play();
            GameObject go = new GameObject($"{nameof(FreezingClick)} Cloud");
            go.transform.position = -MouseHelper.GetMouseWorldPositionInGame();
            go.transform.localScale *= cloudRangeMultiplier;
            go.layer = cloudLayer;

            go.AddComponent<SpriteRenderer>();

            CircleCollider2D collider = go.AddComponent<CircleCollider2D>();
            collider.isTrigger = cloudColliderIsTrigger;

            Animator animator = go.AddComponent<Animator>();
            animator.runtimeAnimatorController = cloudAnimation;
            AnimationClip animationclip = cloudAnimation.animationClips[0];
            animator.speed = animationclip.length / cloudDuration;

            go.AddComponent<Cloud>().SetConfiguration(EffectDuration, effectColor, cloudColliderMultiplier);

            Destroy(go, cloudDuration);
        }

        public class Cloud : MonoBehaviour, IDamageOnTouch<Creature>
        {
            private SpriteRenderer spriteRenderer;
            private CircleCollider2D circleCollider2D;
            private float effectDuration;
            private float scale;
            private Color effectColor;

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
            private void Start()
            {
                spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
            private void Update()
            {
                Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
                circleCollider2D.radius = Mathf.Max(spriteSize.x, spriteSize.y) / 2 * scale;
            }

            public void SetConfiguration(float effectDuration, Color effectColor, float scale = 1)
            {
                this.effectDuration = effectDuration;
                this.scale = scale;
                this.effectColor = effectColor;
            }

            public void ProduceDamage(IHasHealth takeDamage, ITakePush takePush, ITakeEffect<Creature> takeEffect)
            {
                takeEffect?.TakeEffect(new StunEffect(effectDuration, effectColor));
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Used by Unity.")]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancy", "RCS1163:Unused parameter.", Justification = "Used by Unity")]
            private void OnCollisionEnter(Collision collision) => Destroy(gameObject);

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Used by Unity.")]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancy", "RCS1163:Unused parameter.", Justification = "Used by Unity")]
            private void OnTriggerEnter(Collider other) => Destroy(gameObject);
        }
    }
}