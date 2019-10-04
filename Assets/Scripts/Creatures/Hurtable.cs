using System;
using System.Collections;
using FloatPool;
using UnityEngine;

namespace CreaturesAddons
{
    public class Hurtable : MonoBehaviour, ITakeDamage
    {
        [Header("Configuration")]
        [Tooltip("Health.")]
        public Pool health;

        [Header("Setup")]
        [Tooltip("FloatingTextController Script")]
        public FloatingTextController floatingTextController;

        protected IUpdate[] updates;

        protected virtual void Awake()
        {
            health.Initialize();
            updates = new IUpdate[] { health };
        }

        protected virtual void Update() => Array.ForEach(updates, e => e.UpdateBehaviour(Time.deltaTime));

        /// <summary>
        /// Take damage reducing its <see cref="Health"/>.<br>
        /// Animation and floating text will only be show if their parameters are <see langword="true"/> and the effective taken damage is greater than 0.
        /// </summary>
        /// <param name="amount">Amount of <see cref="Health"/> lost. Must be positive.</param>
        /// <param name="displayText">Whenever the damage taken must be shown in a floating text.</param>
        /// <param name="displayAnimation">Whenever it should display <see cref="ANIMATION_STATE_HURT"/> animation.</param>
        public virtual void TakeDamage(float amount, bool displayText = true, bool displayAnimation = true)
        {
            (float remaining, float taken) = health.Decrease(amount);
            if (taken > 0)
            {
                if (displayAnimation)
                    DisplayTakeDamageAnimation();
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
        }

        IEnumerator DestroyOnNextFrame()
        {
            yield return null;
            if (Global.players.Contains(gameObject.transform))
                Global.players.Remove(gameObject.transform);
            Destroy(gameObject);
        }

        protected virtual void DisplayTakeDamageAnimation() { }

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