using UnityEngine;

namespace CreaturesAddons
{
    public class MouseShooter : Weapon
    {
        [Header("Configuration")]
        [SerializeField, Tooltip("Damage on hit.")]
        private float damage = 1;
        [SerializeField, Tooltip("Push strength on hit.")]
        private float pushStrength = 0;

        [Header("Projectile Configuration")]
        [SerializeField, Tooltip("Projectile force.")]
        private float projectileForce = 10;
        [SerializeField, Tooltip("Gravity scale of projectile.")]
        private float projectileGravity = 1;
        [SerializeField, Tooltip("Whenever if projectile collider is trigger.")]
        private bool projectileColliderIsTrigger = false;
        [SerializeField, Tooltip("Projectile scale multiplier.")]
        private float projectileScaleMultiplier = 1;
        [SerializeField, Tooltip("Projectile duration in seconds.")]
        private float projectileDuration = 1;
        [SerializeField, Tooltip("Projectile layer."), Layer]
        private int projectileLayer = 0;

        [Header("Setup")]
#pragma warning disable CS0649
        [SerializeField, DrawVectorRelativeToTransform]
        private Vector2 shootingPosition;
#pragma warning restore CS0649
        [SerializeField, Tooltip("Animation played by projectile.")]
        public RuntimeAnimatorController projectileAnimation;


        private Transform shootingTransform;
        private SpriteRenderer creatureSpriteRenderer;

        private Vector2 ShootingPoint => new Vector3(creatureSpriteRenderer.flipX ? -shootingPosition.x : shootingPosition.x, shootingPosition.y) + shootingTransform.position;

        public override void Init(Creature creature)
        {
            shootingTransform = creature.Transform;
            creatureSpriteRenderer = creature.GetComponent<SpriteRenderer>();
            base.Init(creature);
        }

        protected override void Attack()
        {
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
            go.AddComponent<Projectile>().SetConfiguration(damage, pushStrength);

            Destroy(go, projectileDuration);
        }
    }

    public class Projectile : PassiveMelee
    {
        private SpriteRenderer spriteRenderer;
        private CircleCollider2D circleCollider2D;

        private void Start()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
        }

        private void Update()
        {
            Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
            circleCollider2D.radius = Mathf.Max(spriteSize.x, spriteSize.y) / 2;
        }

        public void SetConfiguration(float damage, float pushStrength)
        {
            this.damage = damage;
            this.pushStrength = pushStrength;
        }

        public override void ProduceDamage(object victim)
        {
            base.ProduceDamage(victim);
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision) => Destroy(gameObject);
        private void OnTriggerEnter(Collider other) => Destroy(gameObject);
    }
}
