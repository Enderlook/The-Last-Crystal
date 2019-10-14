using System;
using CreaturesAddons.Weapons;
using UnityEngine;

namespace CreaturesAddons
{
    public class Creature : Hurtable, IPush
    {
        [Header("Configuration")]
        [Tooltip("Movement speed.")]
        public float speed = 1;

        [Tooltip("Sprite Renderer Component.")]
        public SpriteRenderer sprite;

        [Tooltip("StoppableRigidbody Script")]
        public StoppableRigidbody stoppableRigidbody;

        [Tooltip("Rigidbody Component.")]
        public Rigidbody2D thisRigidbody2D;

        [Tooltip("Animator Component.")]
        public Animator animator;

        private IMove move;
        private IAttack attack;

        private const string ANIMATION_STATE_HURT = "Hurt";

        public float SpeedMultiplier {
            get => stoppableRigidbody.SpeedMultiplier;
            set => stoppableRigidbody.SpeedMultiplier = value;
        }

        public Transform Transform => thisRigidbody2D.transform;

        protected override void Awake()
        {
            base.Awake();
            LoadComponents();
        }

        private void LoadComponents()
        {
            updates = gameObject.GetComponentsInChildren<IUpdate>();
            move = gameObject.GetComponentInChildren<IMove>();
            attack = gameObject.GetComponentInChildren<IAttack>();
            Array.ForEach(gameObject.GetComponents<IInit>(), e => e.Init(this));
        }

        protected override void Update()
        {
            if (Menu.IsPause)
                return;
            move?.Move(Time.deltaTime, SpeedMultiplier * speed);
            attack?.Attack(Time.time);
            // We don't call base.Update() because that is made in the line below
            Array.ForEach(updates, e => e.UpdateBehaviour(Time.deltaTime));
        }

        /// <summary>
        /// Takes healing increasing its <see cref="Health"/>.
        /// </summary>
        /// <param name="amount">Amount of <see cref="Health"/> recovered. Must be positive.</param>
        public void TakeHealing(float amount) => health.Increase(amount);

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
                if (direction.x > 1 && direction.x < 0) direction.x = -1;
                direction.y = 1; // Assign this value on the Y axis, because the actual value is 0 or less than 1.
            }
                
            thisRigidbody2D.AddForce(direction * force);
        }

        protected override void DisplayTakeDamageAnimation() => animator.SetTrigger(ANIMATION_STATE_HURT);
    }
}