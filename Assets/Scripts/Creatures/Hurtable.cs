﻿using CreaturesAddons.Weapons;

using FloatingText;

using FloatPool;

using ScriptableSound;

using System;
using System.Collections;

using UnityEngine;

namespace CreaturesAddons
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
#pragma warning restore CS0649

        protected IUpdate[] updates;

        protected virtual void Awake()
        {
            hurtSound.Init();
            health.Initialize();
            updates = new IUpdate[] { health, hurtSound };
        }

        protected virtual void Update() => Array.ForEach(updates, e => e.UpdateBehaviour(Time.deltaTime));

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

        protected virtual void TakeDamageFeedback()
        {
            hurtSound.Play();
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

        private void CheckInDamageCollision(GameObject target)
        {
            IDamageOnTouch damageOnTouch = target.gameObject.GetComponent<IDamageOnTouch>();
            if (damageOnTouch != null)
                damageOnTouch.ProduceDamage(this);
        }
    }
}