using CreaturesAddons;

using UnityEngine;

namespace PlayerAddons
{
    public enum DashState { Start, Dashing, Cooldown }

    public class DashEffect : MonoBehaviour, IInit, IUpdate
    {
#pragma warning disable CS0649
        [Header("Setup")]
        [SerializeField, Tooltip("Input for dash.")]
        private KeyCode dashKey;

        [SerializeField, Tooltip("Duration of effect.")]
        private float startDashTime = 1;

        [SerializeField, Tooltip("Velocity dash.")]
        private float forceDash = 1;

        [SerializeField, Tooltip("Effect.")]
        private GameObject effect;

        public DashState dashState;
#pragma warning restore CS0649

        private Rigidbody2D rb2D;
        private SpriteRenderer sprite;
        private Vector2 savedVelocity;
        private float dashTime;
        private GameObject dash;

        void IInit.Init(Creature creature)
        {
            rb2D = creature.ThisRigidbody2D;
            sprite = creature.Sprite;
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
                        dash = Instantiate(effect, new Vector2(rb2D.position.x, rb2D.position.y + 0.15f),
                            transform.rotation);
                        float vel = sprite.flipX ? -forceDash : forceDash;
                        rb2D.velocity = Vector2.right * vel;
                        dashState = DashState.Dashing;
                    }
                    break;
                case DashState.Dashing:
                    if (dashTime <= 0)
                    {
                        Destroy(dash);
                        dashTime = 0;
                        rb2D.velocity = savedVelocity;
                        dashState = DashState.Cooldown;
                    }
                    else
                        dashTime -= deltaTime;
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
