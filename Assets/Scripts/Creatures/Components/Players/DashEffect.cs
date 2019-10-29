using UnityEngine;
using CreaturesAddons;
using Utils;
using System.Collections;

namespace PlayerAddons
{
    public enum DashState { Start, Dashing, Cooldown }

    public class DashEffect : MonoBehaviour, IInit, IUpdate
    {
        [Header("Setup")]
        [SerializeField, Tooltip("Input for dash.")]
        private KeyCode dashKey;
        [SerializeField, Tooltip("Duration of effect.")]
        private float dashTime = 1;
        [SerializeField, Tooltip("Velocity dash.")]
        private float forceDash;

        public DashState dashState;

        private Rigidbody2D rb2D;
        private SpriteRenderer sprite;
        private Vector2 savedVelocity;
        private float maxDash = 2f;

        void IInit.Init(Creature creature)
        {
            rb2D = creature.thisRigidbody2D;
            sprite = creature.sprite;
        }

        void IUpdate.UpdateBehaviour(float deltaTime)
        {
            switch (dashState)
            {
                case DashState.Start:
                    if (Input.GetKey(dashKey))
                    {
                        savedVelocity = rb2D.velocity;
                        float vel = sprite.flipX ? -forceDash : forceDash;
                        rb2D.velocity = Vector2.right * vel;
                        dashState = DashState.Dashing;
                    }
                    break;
                case DashState.Dashing:
                    dashTime += deltaTime * 3;
                    if (dashTime >= maxDash)
                    {
                        dashTime = maxDash;
                        rb2D.velocity = savedVelocity;
                        dashState = DashState.Cooldown;
                    }
                    break;
                case DashState.Cooldown:
                    dashTime -= deltaTime;
                    if (dashTime <= 0)
                    {
                        dashTime = 0;
                        dashState = DashState.Start;
                    }
                    break;
            }
        }
    }
}
