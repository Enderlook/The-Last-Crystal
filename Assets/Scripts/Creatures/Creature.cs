using AdditionalComponents;

using CreaturesAddons.Weapons;

using Master;

using System;
using System.Linq;
using UnityEngine;

namespace CreaturesAddons
{
    public class Creature : Hurtable, IPush
    {
#pragma warning disable CS0649
        [Header("Configuration")]
        [SerializeField, Tooltip("Movement speed.")]
        private float speed = 1;

        [Header("Setup")]
        [SerializeField, Tooltip("Sprite Renderer Component.")]
        private SpriteRenderer sprite;

        public SpriteRenderer Sprite => sprite;

        [SerializeField, Tooltip("StoppableRigidbody Script")]
        private StoppableRigidbody stoppableRigidbody;

        public StoppableRigidbody StoppableRigidbody => stoppableRigidbody;

        [SerializeField, Tooltip("Rigidbody Component.")]
        private Rigidbody2D thisRigidbody2D;

        public Rigidbody2D ThisRigidbody2D => thisRigidbody2D;

        [SerializeField, Tooltip("Animator Component.")]
        private Animator animator;

        public Animator Animator => animator;

        [SerializeField, Tooltip("Ground checker.")]
        private GroundChecker groundChecker;

        public GroundChecker GroundChecker => groundChecker;

        [SerializeField, Tooltip("Material for effect hurt.")]
        private Material redFlash;

#pragma warning restore CS0649

        private IMove move;
        private IAttack attack;

        private Material defMaterial;

        private const string ANIMATION_STATE_HURT = "Hurt";

        public float SpeedMultiplier {
            get => StoppableRigidbody.SpeedMultiplier;
            set => StoppableRigidbody.SpeedMultiplier = value;
        }

        public Transform Transform => ThisRigidbody2D.transform;

        protected override void Awake()
        {
            defMaterial = Sprite.material;
            base.Awake();
            LoadComponents();
        }

        private void LoadComponents()
        {
            updates = updates.Concat(gameObject.GetComponentsInChildren<IUpdate>()).ToArray();
            move = gameObject.GetComponentInChildren<IMove>();
            attack = gameObject.GetComponentInChildren<IAttack>();
            Array.ForEach(gameObject.GetComponents<IInit>(), e => e.Init(this));
        }

        protected override void Update()
        {
            if (Settings.IsPause)
                return;
            move?.Move(Time.deltaTime, SpeedMultiplier * speed);
            attack?.Attack(Time.deltaTime);
            // We don't call base.Update() because that is made in the line below
            Array.ForEach(updates, e => e.UpdateBehaviour(Time.deltaTime));
        }

        /// <summary>
        /// Takes healing increasing its <see cref="Health"/>.
        /// </summary>
        /// <param name="amount">Amount of <see cref="Health"/> recovered. Must be positive.</param>
        public void TakeHealing(float amount) => Health.Increase(amount);

        /// <summary>
        /// Push creature.
        /// </summary>
        /// <param name="direction">Direction to apply force.</param>
        /// <param name="force">Amount of force to apply</param>
        public void Push(Vector2 direction, float force = 1, PushMode pushMode = PushMode.Local)
        {
            if (pushMode == PushMode.Local)
            {
                direction = ((Vector2)Transform.position - direction).normalized;
                // The conditionals verify if the player / enemy is above the object.
                if (direction.x < 1 && direction.x > 0) direction.x = 1;
                if (direction.x > -1 && direction.x < 0) direction.x = -1;
                direction.y = 1; // Assign this value on the Y axis, because the actual value is 0 or less than 1.
            }

            ThisRigidbody2D.AddForce(direction * force);
        }

        protected override void DisplayTakeDamageAnimation()
        {
            Sprite.material = redFlash;
            Invoke(nameof(ResetMaterial), .1f);
        }

        private void ResetMaterial() => Sprite.material = defMaterial;
    }
}