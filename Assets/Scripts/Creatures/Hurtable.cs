using Additions.Components.ColorCombiner;
using Additions.Components.FloatPool;
using Additions.Components.ScriptableSound;
using Additions.Prefabs.FloatingText;
using Additions.Utils;

using Creatures.Weapons;

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Creatures
{
    public class Hurtable : MonoBehaviour, ITakeDamage
    {
#pragma warning disable CS0649
        [Header("Setup")]
        [SerializeField, Tooltip("Health.")]
        private Pool health;

        public Pool Health => health;

        [SerializeField, Tooltip("FloatingTextController Script")]
        private FloatingTextController floatingTextController;

        [SerializeField, Tooltip("Sound played when hurt.")]
        private SoundPlay hurtSound;

        [SerializeField, Tooltip("Sound played on death.")]
        private Sound dieSound;

        [SerializeField, Tooltip("Sprite Renderer Component.")]
        private SpriteRenderer sprite;

        public SpriteRenderer Sprite => sprite;

        [SerializeField, Tooltip("Sprite colorer.")]
        protected SpriteRenderColorerWithTimer spriteColorer;

        [SerializeField, Tooltip("Color tint for Sprite Renderer used when hurt.")]
        private Color hurtColor = Color.red;
#pragma warning restore CS0649

        protected HashSet<IUpdate> updates;

        protected virtual void Awake()
        {
            hurtSound.Init();
            health.Initialize();
            spriteColorer.Initialize();
            updates = new HashSet<IUpdate>
            {
                health,
                hurtSound,
                spriteColorer
            };
        }

        protected virtual void Update()
        {
            foreach (IUpdate update in updates)
                update.UpdateBehaviour(Time.deltaTime);
        }

        /// <summary>
        /// Take damage reducing its <see cref="Health"/>.<br>
        /// Animation and floating text will only be show if their parameters are <see langword="true"/> and the effective taken damage is greater than 0.
        /// </summary>
        /// <param name="amount">Amount of <see cref="Health"/> lost. Must be positive.</param>
        /// <param name="displayText">Whenever the damage taken must be shown in a floating text.</param>
        /// <param name="produceFeedback">Whenever it should display <see cref="ANIMATION_STATE_HURT"/> animation and play <see cref="hurtSound"/> sound.</param>
        public virtual void TakeDamage(float amount, bool displayText = true, bool produceFeedback = true)
        {
            (_, float taken) = health.Decrease(amount);
            if (taken > 0)
            {
                if (produceFeedback)
                    TakeDamageFeedback();
                if (displayText)
                    SpawnFloatingText(amount, Color.Lerp(Color.red, new Color(1, .5f, 0), health.Ratio));
                if (health.Current <= 0)
                    Die();
            }
        }

        /// <summary>
        /// Disables <see cref="gameObject"/> and spawn an explosion prefab instance on current location.
        /// </summary>
        /// <param name="suicide"><see langword="true"/> if it was a suicide. <see langword="false"/> if it was murderer.</param>
        public virtual void Die(bool suicide = false)
        {
            Array.ForEach(gameObject.GetComponentsInChildren<IDie>(), e => e.Die(suicide));
            StartCoroutine(DestroyOnNextFrame());
            SimpleSoundPlayer.CreateOneTimePlayer(dieSound, true, true);
        }

        private IEnumerator DestroyOnNextFrame()
        {
            yield return null;
            Destroy(gameObject);
        }

        /// <summary>
        /// Add a color to <see cref="spriteColorer"/>.
        /// </summary>
        /// <param name="color">Color to add.</param>
        public void AddColorTint(Color color) => spriteColorer.Add(color);

        /// <summary>
        /// Remove a color from <see cref="spriteColorer"/>.
        /// </summary>
        /// <param name="color">Color to remove.</param>
        public void RemoveColorTint(Color color) => spriteColorer.Remove(color);

        /// <summary>
        /// Add a color to <see cref="spriteColorer"/>.
        /// </summary>
        /// <param name="color">Color to add.</param>
        /// <param name="duration">Duration of color in seconds.</param>
        public void AddColorTint(Color color, float duration) => spriteColorer.Add(color, duration);

        protected virtual void TakeDamageFeedback()
        {
            hurtSound.Play();
            AddColorTint(hurtColor, .1f);
        }

        /// <summary>
        /// Spawn a floating text above the creature.
        /// </summary>
        /// <param name="text">Text to display.</param>
        /// <param name="textColor">Color of the text.</param>
        /// <seealso cref="SpawnFloatingText(float, Color?, bool)"/>
        protected void SpawnFloatingText(string text, Color? textColor)
        {
            if (floatingTextController != null)
                floatingTextController.SpawnFloatingText(text, textColor);
        }

        /// <summary>
        /// Spawn a floating text above the creature.
        /// </summary>
        /// <param name="value">Text to display.</param>
        /// <param name="textColor">Color of the text.</param>
        /// <param name="checkIfPositive">Only display if the number is positive.</param>
        /// <seealso cref="SpawnFloatingText(string, Color?, bool)"/>
        protected void SpawnFloatingText(float value, Color? textColor, bool checkIfPositive = true)
        {
            if (!checkIfPositive || value > 0)
                SpawnFloatingText(value.ToString(), textColor);
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision) => CheckInDamageCollision(collision.gameObject);

        protected virtual void OnTriggerEnter2D(Collider2D collision) => CheckInDamageCollision(collision.gameObject);

        protected virtual void CheckInDamageCollision(GameObject target)
        {
            IDamageOnTouch<Creature> damageOnTouch = target.gameObject.GetComponent<IDamageOnTouch<Creature>>();
            if (damageOnTouch != null)
                damageOnTouch.ProduceDamage(this, null, null);
        }
    }
}