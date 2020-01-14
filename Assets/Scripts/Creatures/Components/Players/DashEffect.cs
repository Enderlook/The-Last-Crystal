using Additions.Components.FloatPool;
using Additions.Components.ScriptableSound;
using Additions.Utils;

using Creatures;

using UnityEngine;

namespace PlayerAddons
{
    public class DashEffect : MonoBehaviour, IInit, IUpdate
    {
#pragma warning disable CS0649
        [Header("Configuration")]
        [SerializeField, Tooltip("Duration of effect.")]
        private float duration = 1;

        [SerializeField, Tooltip("Velocity dash.")]
        private float forceDash = 1;

        [SerializeField, Tooltip("Energy consumed by dash.")]
        private float energyCost = 5;

        [Header("Setup")]
        [SerializeField, Tooltip("Input for dash.")]
        private KeyCode dashKey;

        [SerializeField, Tooltip("Effect.")]
        private GameObject effect;

        [SerializeField, Tooltip("Energy pool.")]
        private Pool energyPool;

        [SerializeField, Tooltip("Melee used when dashing.")]
        private ActiveMelee melee;

        [SerializeField, Tooltip("Dash sound.")]
        private SoundPlay dashSound;
#pragma warning restore CS0649

        private Rigidbody2D thisRigidbody2D;
        private SpriteRenderer sprite;
        private float remaining_time;
        private bool isDashing;

        void IInit.Init(Creature creature)
        {
            thisRigidbody2D = creature.ThisRigidbody2D;
            sprite = creature.Sprite;
            dashSound.Init();
        }

        private Vector2 GetVelocityBonus() => (sprite.flipX ? -forceDash : forceDash) * Vector2.right;

        void IUpdate.UpdateBehaviour(float deltaTime)
        {
            if (dashSound != null)
                dashSound.UpdateBehaviour(deltaTime);
            if (melee != null)
                melee.UpdateBehaviour(deltaTime);
            if (isDashing)
            {
                remaining_time -= deltaTime;
                if (remaining_time <= 0)
                {
                    isDashing = false;
                    thisRigidbody2D.velocity = Vector2.zero;
                    if (melee != null)
                    {
                        melee.IsEnabled = false;
                        melee.Recharge(deltaTime);
                    }
                }
            }
            else if (Input.GetKey(dashKey) && energyPool.Current >= energyCost)
            {
                energyPool.Decrease(energyCost);

                // Spawn particles
                Destroy(Instantiate(effect, new Vector2(thisRigidbody2D.position.x, thisRigidbody2D.position.y + 0.15f), transform.rotation), duration);

                thisRigidbody2D.velocity += GetVelocityBonus();
                isDashing = true;
                remaining_time = duration;
                if (melee != null)
                {
                    melee.IsEnabled = true;
                    melee.SetReady();
                }
                if (dashSound != null)
                    dashSound.Play();
            }
        }
    }
}
