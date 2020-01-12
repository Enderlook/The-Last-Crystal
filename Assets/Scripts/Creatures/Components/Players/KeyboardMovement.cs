using Creatures;

using UnityEngine;

namespace PlayerAddons
{
    public class KeyboardMovement : MonoBehaviour, IMove, IInit
    {
#pragma warning disable CS0649
        [Header("Configuration")]
        [SerializeField, Tooltip("Move right key.")]
        private KeyCode rightKey;

        [SerializeField, Tooltip("Move left key.")]
        private KeyCode leftKey;

        [SerializeField, Tooltip("Movement speed.")]
        private float speed;

        [SerializeField, Tooltip("Jump key.")]
        private KeyCode jumpKey;

        [SerializeField, Tooltip("Maximum number of jumps.")]
        private int maxJumps;

        private int remainingJumps;

        [SerializeField, Tooltip("Jump strength.")]
        private float jumpStrength;

        [SerializeField, Tooltip("Move force horizontal.")]
        private float moveForce;
#pragma warning restore CS0649

        private Rigidbody2D thisRigidbody2D;
        private Animator thisAnimator;
        private SpriteRenderer thisSprite;

        private GroundChecker groundChecker;

        private static class ANIMATION_STATES
        {
            public const string
                WALK = "Walk",
                JUMP = "Jump";
        }

        void IInit.Init(Creature creature)
        {
            thisRigidbody2D = creature.ThisRigidbody2D;
            thisAnimator = creature.Animator;
            thisSprite = creature.Sprite;
            groundChecker = creature.GroundChecker;
            remainingJumps = maxJumps;
        }

        void IMove.Move(float deltaTime, float speedMultiplier)
        {
            if (groundChecker.IsGrounded())
            {
                remainingJumps = maxJumps;
                thisAnimator.SetBool(ANIMATION_STATES.JUMP, false);
            }

            thisAnimator.SetFloat(ANIMATION_STATES.WALK, Mathf.Abs(thisRigidbody2D.velocity.x));

            if (Input.GetKey(rightKey))
                MoveHorizontal(deltaTime * speedMultiplier);
            if (Input.GetKey(leftKey))
                MoveHorizontal(-deltaTime * speedMultiplier);

            if (Input.GetKeyDown(jumpKey) && remainingJumps > 0)
            {
                thisRigidbody2D.AddForce(thisRigidbody2D.transform.up * jumpStrength * thisRigidbody2D.mass);
                remainingJumps--;
                thisAnimator.SetBool(ANIMATION_STATES.JUMP, true);
            }
        }

        private void MoveHorizontal(float distance)
        {
            thisSprite.flipX = distance < 0;
            thisRigidbody2D.AddForce(thisRigidbody2D.transform.right * distance * moveForce);
            if (Mathf.Abs(thisRigidbody2D.velocity.x) > speed)
                thisRigidbody2D.velocity = new Vector2(Mathf.Sign(thisRigidbody2D.velocity.x) * speed, thisRigidbody2D.velocity.y);
        }
    }
}