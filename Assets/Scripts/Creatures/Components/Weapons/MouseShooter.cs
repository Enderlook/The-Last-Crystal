﻿using AdditionalAttributes;

using Master;

using UnityEngine;

using Utils;

namespace CreaturesAddons.Weapons
{
    public class MouseShooter : Weapon
    {
        [Header("Configuration")]
        [SerializeField, Tooltip("Damage on hit.")]
        private float damage = 1;
#pragma warning disable CS0649
        [SerializeField, Tooltip("Push strength on hit.")]
        private float pushStrength;
        [SerializeField, Tooltip("Animation attack name.")]
        private string animationName;
#pragma warning restore CS0649

        [Header("Projectile Configuration")]
        [SerializeField, Tooltip("Projectile force.")]
        private float projectileForce = 10;
        [SerializeField, Tooltip("Gravity scale of projectile.")]
        private float projectileGravity = 1;
#pragma warning disable CS0649
        [SerializeField, Tooltip("Whenever if projectile collider is trigger.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Unity Editor can't handle readonly.")]
        private bool projectileColliderIsTrigger;
#pragma warning restore CS0649
        [SerializeField, Tooltip("Projectile scale multiplier.")]
        private float projectileScaleMultiplier = 1;
        [SerializeField, Tooltip("Projectile duration in seconds.")]
        private float projectileDuration = 1;
#pragma warning disable CS0649
        [SerializeField, Tooltip("Projectile layer."), Layer]
        private int projectileLayer;

        [Header("Setup")]
        [SerializeField, DrawVectorRelativeToTransform]
        private Vector2 shootingPosition;
        [SerializeField, Tooltip("Animation played by projectile.")]
        private RuntimeAnimatorController projectileAnimation;
        [SerializeField, Tooltip("Projectile collider scale.")]
        private float projectileColliderScale;
#pragma warning restore CS0649

        private Transform shootingTransform;
        private SpriteRenderer creatureSpriteRenderer;
        private Animator thisAnimator;

        private Vector2 ShootingPoint => new Vector3(creatureSpriteRenderer.flipX ? -shootingPosition.x : shootingPosition.x, shootingPosition.y) + shootingTransform.position;

        public override void Init(Creature creature)
        {
            shootingTransform = creature.Transform;
            thisAnimator = creature.animator;
            creatureSpriteRenderer = creature.GetComponent<SpriteRenderer>();
            base.Init(creature);
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
            Global.audioSystem.ShootSoundEffect();
            GameObject go = new GameObject($"{nameof(MouseShooter)} Projectile");
            go.transform.position = ShootingPoint;
            go.transform.localScale *= projectileScaleMultiplier;
            go.layer = projectileLayer;

            Rigidbody2D rigidbody = go.AddComponent<Rigidbody2D>();
            rigidbody.AddForce((MouseHelper.GetMousePositionInGame() - ShootingPoint).normalized * projectileForce);
            rigidbody.gravityScale = projectileGravity;

            go.AddComponent<SpriteRenderer>();

            CircleCollider2D collider = go.AddComponent<CircleCollider2D>();
            collider.isTrigger = projectileColliderIsTrigger;

            go.AddComponent<Animator>().runtimeAnimatorController = projectileAnimation;
            go.AddComponent<Projectile>().SetConfiguration(damage, pushStrength, projectileColliderScale);

            Destroy(go, projectileDuration);
        }
    }

    public class Projectile : PassiveMelee
    {
        private SpriteRenderer spriteRenderer;
        private CircleCollider2D circleCollider2D;
        private float scale;

        private void Start()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
        }

        private void Update()
        {
            Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
            circleCollider2D.radius = Mathf.Max(spriteSize.x, spriteSize.y) / 2 * scale;
        }

        public void SetConfiguration(float damage, float pushStrength, float scale = 1)
        {
            this.damage = damage;
            this.pushStrength = pushStrength;
            this.scale = scale;
            thisTransform = transform;
        }

        public override void ProduceDamage(object victim)
        {
            base.ProduceDamage(victim);
            Destroy(gameObject);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Unity's method.")]
        private void OnCollisionEnter(Collision collision) => Destroy(gameObject);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Unity's method.")]
        private void OnTriggerEnter(Collider other) => Destroy(gameObject);
    }
}
