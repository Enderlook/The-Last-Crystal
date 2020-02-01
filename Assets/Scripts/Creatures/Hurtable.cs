using Additions.Components.ColorCombiner;
using Additions.Components.FloatPool;
using Additions.Components.FloatPool.Decorators;
using Additions.Components.ScriptableSound;
using Additions.Prefabs.FloatingText;
using Additions.Prefabs.HealthBarGUI;
using Additions.Utils;

using Creatures.Weapons;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures
{
    public class Hurtable : MonoBehaviour, IHasHealth
    {
#pragma warning disable CS0649
        [Header("Setup")]
        [SerializeField, Tooltip("Health.")]
        private Pool health;

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

        [SerializeField, Tooltip("Color tint for Sprite Renderer used when healed.")]
        private Color healingSpriteColor = Color.green;

        [SerializeField, Tooltip("Color of floating text when healed.")]
        private Color healingTextColor = Color.green;
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

        public void SetHealthBar(HealthBar energyBar) => health.GetLayer<BarDecorator>().Bar = energyBar;

        protected virtual void Update()
        {
            foreach (IUpdate update in updates)
                update.UpdateBehaviour(Time.deltaTime);
        }

        /// <inheritdoc />
        public virtual void TakeDamage(float amount, bool displayText = true, bool produceFeedback = true)
        {
            (_, float taken) = health.Decrease(amount);
            if (taken > 0)
            {
                if (produceFeedback)
                {
                    hurtSound.Play();
                    AddColorTint(hurtColor, .1f);
                }
                if (displayText)
                    SpawnFloatingText(amount, Color.Lerp(Color.red, new Color(1, .5f, 0), health.Ratio));
                if (health.Current <= 0)
                    Die();
            }
        }

        /// <inheritdoc />
        public void TakeHealing(float amount, bool displayText = true, bool produceFeedback = true)
        {
            (_, float increased) = health.Increase(amount);
            if (increased > 0)
            {
                if (produceFeedback)
                    AddColorTint(healingSpriteColor, .1f);
                if (displayText)
                    SpawnFloatingText(amount, healingTextColor);
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