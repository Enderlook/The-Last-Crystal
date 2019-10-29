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
        private float startDashTime = 1;
        [SerializeField, Tooltip("Velocity dash.")]
        private float forceDash = 1;

        public DashState dashState;

        private Rigidbody2D rb2D;
        private SpriteRenderer sprite;
        private Vector2 savedVelocity;
        private float dashTime;

        void IInit.Init(Creature creature)
        {
            rb2D = creature.thisRigidbody2D;
            sprite = creature.sprite;
            dashTime = startDashTime;
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
                    if (dashTime <= 0)
                    {
                        dashTime = 0;
                        rb2D.velocity = savedVelocity;
                        dashState = DashState.Cooldown;
                    }
                    else dashTime -= deltaTime;
                    break;
                case DashState.Cooldown:
                    dashTime += deltaTime;
                    if (dashTime >= startDashTime)
                    {
                        dashTime = startDashTime;
                        dashState = DashState.Start;
                    }
                    break;
            }
        }
    }
}
