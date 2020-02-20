using Additions.Attributes;
using Additions.Components.ScriptableSound;
using Additions.Serializables.Atoms;
using Additions.Serializables.Atoms.Premades.System;
using Additions.Utils;

using Creatures.Effects;

using UnityEngine;

namespace Creatures.Weapons
{
    public class MouseShooter : Weapon
    {
#pragma warning disable CS0649
        [Header("Configuration")]
        [SerializeField, Tooltip("Damage on hit."), Expandable, RestrictType(typeof(IGet<float>))]
        private Atom damage;

        private IGet<float> Damage;

        [SerializeField, Tooltip("Push strength on hit.")]
        private float pushStrength;

        [SerializeField, Tooltip("Animation attack name.")]
        private string animationName;

        [Header("Projectile Configuration")]
        [SerializeField, Tooltip("Projectile force.")]
        private float projectileForce = 10;

        [SerializeField, Tooltip("Gravity scale of projectile.")]
        private float projectileGravity = 1;

        [SerializeField, Tooltip("Whenever if projectile collider is trigger.")]
        private bool projectileColliderIsTrigger;

        [SerializeField, Tooltip("Projectile scale multiplier.")]
        private float projectileScaleMultiplier = 1;

        [SerializeField, Tooltip("The size of the collider is equal to the sprite size multiplicated by this value.")]
        private float projectileColliderMultiplier = 1;

        [SerializeField, Tooltip("Projectile duration in seconds.")]
        private float projectileDuration = 1;

        [SerializeField, Tooltip("Projectile layer."), Layer]
        private int projectileLayer;

        [Header("Setup")]
        [SerializeField, DrawVectorRelativeToTransform]
        private Vector2 shootingPosition;

        [SerializeField, Tooltip("Animation played by projectile.")]
        private RuntimeAnimatorController projectileAnimation;

        [SerializeField, Tooltip("Shooting sound.")]
        private SoundPlay shootingSound;
#pragma warning restore CS0649

        private Transform shootingTransform;
        private SpriteRenderer creatureSpriteRenderer;
        private Animator thisAnimator;

        private Vector2 ShootingPoint => new Vector3(creatureSpriteRenderer.flipX ? -shootingPosition.x : shootingPosition.x, shootingPosition.y) + shootingTransform.position;

        public override void Initialize(Creature creature)
        {
            shootingTransform = creature.Transform;
            thisAnimator = creature.Animator;
            creatureSpriteRenderer = creature.GetComponent<SpriteRenderer>();
            shootingSound.Init();
            Damage = (IGet<float>)damage;
            base.Initialize(creature);
        }

        public override void UpdateBehaviour(float deltaTime)
        {
            shootingSound.UpdateBehaviour(deltaTime);
            base.UpdateBehaviour(deltaTime);
        }

        protected override void Attack()
        {
            if (thisAnimator == null || string.IsNullOrEmpty(animationName))
                Shoot();
            else
                thisAnimator.SetTrigger(animationName);
        }

        private void Shoot()
        {
            shootingSound.Play();
            GameObject go = new GameObject($"{nameof(MouseShooter)} Projectile");
            go.transform.position = ShootingPoint;
            go.transform.localScale *= projectileScaleMultiplier;
            go.layer = projectileLayer;

            Rigidbody2D rigidbody = go.AddComponent<Rigidbody2D>();
            rigidbody.AddForce((-MouseHelper.GetMouseWorldPositionInGame() - ShootingPoint).normalized * projectileForce);
            rigidbody.gravityScale = projectileGravity;

            go.AddComponent<SpriteRenderer>();

            CircleCollider2D collider = go.AddComponent<CircleCollider2D>();
            collider.isTrigger = projectileColliderIsTrigger;

            Animator animator = go.AddComponent<Animator>();
            animator.runtimeAnimatorController = projectileAnimation;
            AnimationClip animationclip = projectileAnimation.animationClips[0];
            animator.speed = animationclip.length / projectileDuration;

            go.AddComponent<Projectile>().SetConfiguration(Damage.Value, pushStrength, projectileColliderMultiplier);

            Destroy(go, projectileDuration);
        }
    }

    public class Projectile : PassiveMelee
    {
        private SpriteRenderer spriteRenderer;
        private CircleCollider2D circleCollider2D;
        private float scale;

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

        public void SetConfiguration(float damage, float pushStrength, float scale = 1)
        {
            Damage = damage;
            this.pushStrength = pushStrength;
            this.scale = scale;
            thisTransform = transform;
        }

        public override void ProduceDamage(IHasHealth takeDamage, ITakePush takePush, ITakeEffect<Creature> takeEffect)
        {
            base.ProduceDamage(takeDamage, takePush, takeEffect);
            Destroy(gameObject);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Used by Unity.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancy", "RCS1163:Unused parameter.", Justification = "Used by Unity")]
        private void OnCollisionEnter(Collision collision) => Destroy(gameObject);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Used by Unity.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancy", "RCS1163:Unused parameter.", Justification = "Used by Unity")]
        private void OnTriggerEnter(Collider other) => Destroy(gameObject);
    }
}
