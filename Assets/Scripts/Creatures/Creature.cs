using Additions.Utils;

using Creatures.Effects;
using Creatures.Weapons;

using Master;

using System;

using UnityEngine;

namespace Creatures
{
    public class Creature : Hurtable, ITakePush, ITakeEffect<Creature>
    {
#pragma warning disable CS0649
        [Header("Configuration")]
        [SerializeField, Tooltip("Movement speed.")]
        private float speed = 1;

        [Header("Setup")]
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
#pragma warning restore CS0649

        private IMove move;
        private IAttack attack;

        private EffectManager<Creature> effectManager;

        [NonSerialized]
        public bool isStunned;

        public float SpeedMultiplier {
            get => StoppableRigidbody.SpeedMultiplier;
            set => StoppableRigidbody.SpeedMultiplier = value;
        }

        public Transform Transform => ThisRigidbody2D.transform;

        protected override void Awake()
        {
            base.Awake();
            LoadComponents();
        }

        private void LoadComponents()
        {
            effectManager = new EffectManager<Creature>(this);
            updates.UnionWith(gameObject.GetComponentsInChildren<IUpdate>());
            updates.Add(effectManager);
            move = gameObject.GetComponentInChildren<IMove>();
            attack = gameObject.GetComponentInChildren<IAttack>();
            Array.ForEach(gameObject.GetComponents<IInitialize<Creature>>(), e => e.Initialize(this));
        }

        protected override void Update()
        {
            if (Settings.IsPause)
                return;
            if (!isStunned)
            {
                move?.Move(Time.deltaTime, SpeedMultiplier * speed);
                attack?.Attack(Time.deltaTime);
            }
            // We don't call base.Update() because that is made in the line below
            foreach (IUpdate update in updates)
                update.UpdateBehaviour(Time.deltaTime);
        }

        /// <summary>
        /// Push creature.
        /// </summary>
        /// <param name="direction">Direction to apply force.</param>
        /// <param name="force">Amount of force to apply</param>
        public void TakePush(Vector2 direction, float force = 1, PushMode pushMode = PushMode.Local)
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

        protected override void CheckInDamageCollision(GameObject target)
        {
            IDamageOnTouch<Creature> damageOnTouch = target.gameObject.GetComponent<IDamageOnTouch<Creature>>();
            if (damageOnTouch != null)
                damageOnTouch.ProduceDamage(this, this, this);
        }

        /// <summary>
        /// Take an effect.
        /// </summary>
        /// <param name="effect">Effect to take.</param>
        public void TakeEffect(Effect<Creature> effect) => effectManager.AddEffect(effect);
    }
}
